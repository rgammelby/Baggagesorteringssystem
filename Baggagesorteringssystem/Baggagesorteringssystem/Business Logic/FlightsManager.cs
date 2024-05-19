using Baggagesorteringssystem.Data_Access;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Business_Logic
{
    internal class FlightsManager
    {
        private Connection _cs = new Connection();

        //private List<Flight> _flightsToDeparture;
        private List<Gate> _gates;
        private List<FrontDesk> _frontDesks;
        private Random _random;
        // initialize terminals
        public Terminal terminalA = new Terminal(1, "A");
        public Terminal terminalB = new Terminal(2, "B");

        public Queue<Flight> flightQueue = new Queue<Flight>();


        public List<Flight> FlightsToDeparture { get; set; }
        public List<Gate> Gates { get; set; }
        
        public FlightsManager()
        {
            _random = new Random();
            InitializeGatesAndFrontDesks();
        }

        public List<string> GetUniqueFlightDates()
        {
            List<string> uniqueDates = new List<string>();

            MySqlConnection connection = _cs.GetOpenConenction();
            if (connection != null)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT DATE(departure_time) FROM flights", connection);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = reader.GetDateTime(0);
                        uniqueDates.Add(date.ToString("yyyyMMdd"));
                    }
                }
                connection.Close();
            }

            return uniqueDates;
        }
        public List<Flight> AssignFrontDeskAndGateToFlightThread(string dateString)
        {
            // Create a new object for synchronization
            object lockObject = new object();

            bool keepRunning = true;    
            List<Flight> flightsOfTheDay = null;

            do
            {
                Monitor.Enter(lockObject);
                try 
                { 
                    flightsOfTheDay = GetFlightsByDate(dateString);

                    // Assign front desk and gate to each flight
                    foreach (var flight in flightsOfTheDay)
                    {
                        // Assign a Terminal
                        flight.Terminal = _random.Next(2) == 0 ? terminalA : terminalB;

                        // Assign a gate
                        for (int i = 0; i < _gates.Count; i++)
                        {
                            if (!_gates[i].IsOpen && _gates[i].Terminal == flight.Terminal)
                            {
                                flight.Gate = _gates[i];
                                _gates[i].IsOpen = true;
                                break;
                            }
                        }

                        // Check if there is no gate available
                        if (flight.Gate == null)
                        {
                            Console.WriteLine("No gate available for flight " + flight.FlightNumber);
                            flightQueue.Enqueue(flight);
                            continue;
                        }

                        // Assign a FrontDesk
                        for (int i = 0; i < _frontDesks.Count; i++)
                        {
                            if (!_frontDesks[i].IsOpen && _frontDesks[i].Terminal == flight.Terminal)
                            {
                                flight.FrontDesk = _frontDesks[i];
                                _frontDesks[i].IsOpen = true;
                                break;
                            }
                        }
                    }

                    // check if all flights of the day are depatured
                    if(flightsOfTheDay.All(Flight => Flight.IsDepartured))
                    {
                        keepRunning = false;
                    }
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }

                // Sleep for 1 second
                Thread.Sleep(1000);
            }
            while(keepRunning);

            return flightsOfTheDay;
        }

        //debuge method
        public void AssignFrontDeskAndGateToFlight(List<Flight> flightsOfTheDay)
        {

            // Assign front desk and gate to each flight
            foreach (var flight in flightsOfTheDay)
            {
                // Assign a Terminal
                flight.Terminal = _random.Next(2) == 0 ? terminalA : terminalB; 

                // Assign a gate

                var gate = _gates.FirstOrDefault(g => !g.IsOpen && g.Terminal == flight.Terminal);
                if (gate != null)
                {
                    flight.Gate = gate;
                    gate.IsOpen = true;
                }


                var frontDesk = _frontDesks.FirstOrDefault(f => !f.IsOpen && f.Terminal == flight.Terminal);
                if (frontDesk != null)
                {
                    flight.FrontDesk = frontDesk;
                    frontDesk.Flight = flight;
                    frontDesk.IsOpen = true;
                }

            }

        }
        private void InitializeGatesAndFrontDesks()
        {
            // Generates gates for terminal A
            _gates = new List<Gate>();
            for(int i = 1; i < 20; i++)
            {
                Gate gate = new Gate(terminalA, i, i.ToString());
                _gates.Add(gate);
            } 
            // Generates gates for terminal B
            for (int i = 1; i < 11; i++)
            {
                Gate gate = new Gate(terminalB, i, i.ToString());
                _gates.Add(gate);
            }

            // Generates front desks for Terminal A and B
            _frontDesks = new List<FrontDesk>();
            for (int i = 1; i < 10; i++)
            {
                FrontDesk frontDesk = new FrontDesk(terminalA, i);
                _frontDesks.Add(frontDesk);
            }
            for (int i = 1; i < 20; i++)
            {
                FrontDesk frontDesk = new FrontDesk(terminalB, i);
                _frontDesks.Add(frontDesk);
            }

        }

        // Parse date from yyyyMMdd to Date
        private DateTime ParseDate(string date)
        {
            return DateTime.ParseExact(date, "yyyyMMdd", null);
        }

        // Get a list of flights by date
        public List<Flight> GetFlightsByDate(string dateString)
        {
            DateTime date = ParseDate(dateString);

            List<Flight> flightsOnADay = new List<Flight>();

            // Get all flights on a day in db table flights
            MySqlConnection connection = _cs.GetOpenConenction();
            if (connection != null)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM flights WHERE DATE(departure_time) = @date", connection);
                cmd.Parameters.AddWithValue("@date", date);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Create a new flight object
                        Flight flight = new Flight(
                            reader.GetInt32("flight_id"),
                            reader.GetString("flight_number"),
                            reader.GetDateTime("departure_time"),
                            reader.GetDateTime("arrival_time"),
                            reader.GetInt32("airline_id"),
                            reader.GetInt32("aircraft_id"),
                            reader.GetInt32("destination_id")
                            );

                        flightsOnADay.Add(flight);
                    }
                }
                connection.Close();
            }
            
            return flightsOnADay;
        }


        public void PrintFlightsInformation(List<Flight> flights)
        {
            foreach (Flight flight in flights)
            {
                Console.WriteLine($"Date: {flight.DepartureTime.Date}");
                Console.WriteLine($"Flight Name: {flight.FlightNumber}"); // Assuming FlightNumber is the flight name
                Console.WriteLine($"Departure Time: {flight.DepartureTime.TimeOfDay}");
                Console.WriteLine($"Arrival Time: {flight.ArrivalTime.TimeOfDay}");
                Console.WriteLine($"Terminal: Terminal {flight.Terminal.TerminalName}");
                Console.WriteLine($"Gate: Gate {flight.Gate.GateName}");
                Console.WriteLine($"Front Desk: Front Desk {flight.FrontDesk.FrontDeskId}");
                Console.WriteLine();
            }
        }

    }
}
