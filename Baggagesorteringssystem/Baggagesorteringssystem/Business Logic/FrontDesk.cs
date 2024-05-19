using Baggagesorteringssystem.Data_Access;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Business_Logic
{
    internal class FrontDesk
    {
        private Connection _cs = new Connection();
        
        // create a new FrontDesk object, the Passengers, Luggages, and BoardingPasses lists
        // are all initialized as empty lists, and you can start adding items to them right away.
        // list of CheckedInpassengers
        public List<Passenger> CheckedInPassengers = new List<Passenger>();

        // list of LuggagesToSort, ready to send to the sorting system
        public List<Luggage> LuggagesToSort = new List<Luggage>();

        // list of bording pass for each passenger
        List<BoardingPass> BoardingPasses = new List<BoardingPass>();

       

        private Flight _flight;
        private Terminal _terminal;
        private int _frontDeskId;
        public Terminal Terminal { get; set; }
        public int FrontDeskId { get { return _frontDeskId; } }
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; }
        }
        [JsonIgnore]
        public Flight Flight { get; set; }

        // constructor for the front desk
        public FrontDesk(Terminal teminal, int frontDeskId)
        {
            Terminal = teminal;
            _frontDeskId = frontDeskId;
            _isOpen = false;
        }

        //debug method
        public void CheckInPassengersAndLuggage()
        {
            // Create a new object for synchronization
            object lockObject = new object();

            List<Passenger> checkedInPassengers = GetPassengersOnFlight(Flight);

            Monitor.Enter(lockObject);
            try
            {
                foreach (Passenger passenger in checkedInPassengers)
                {
                    Luggage luggage = new Luggage(Flight.FlightNumber, passenger.PassengerId, Flight.DestinationId, Flight.DepartureTime);

                    CheckInLuggage(luggage);

                    GenerateBoardingPass(passenger, luggage);

                    CheckInPassenger(passenger);
                }
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        public void CheckInPassengersAndLuggageThread()
        {
            // Create a new object for synchronization
            object lockObject = new object();

            bool keepRunning = true;
            List<Passenger> checkedInPassengers = null;
            List<Luggage> luggagesToSort = null;

            do
            {
                Monitor.Enter(lockObject);
                try
                {
                    checkedInPassengers = GetPassengersOnFlight(Flight);

                    foreach (Passenger passenger in checkedInPassengers)
                    {
                        Luggage luggage = new Luggage(Flight.FlightNumber, passenger.PassengerId, Flight.DestinationId, Flight.DepartureTime);
                        
                        CheckInLuggage(luggage);

                        GenerateBoardingPass(passenger, luggage);

                        CheckInPassenger(passenger);
                    }
                    
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }

                // check if all passengers and luggages have been checked in
                if (checkedInPassengers.Count == 0 && luggagesToSort.Count == 0)
                {
                    keepRunning = false;
                }
            }
            while(keepRunning);
           
        }

        // Method to get all passengers on a flight, return a list of passengers
        private List<Passenger> GetPassengersOnFlight(Flight flight)
        {
            List<Passenger> passengers = new List<Passenger>();

            // Create a new connection object
            Connection connection = new Connection();

            // Open the connection
            MySqlConnection conn = connection.GetOpenConenction();

            // Create a new command
            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Passengers WHERE flight_id = {flight.FlightId}", conn);

            // Execute the command and retrieve the data
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Create a new Passenger object for each row
                    Passenger passenger = new Passenger(
                        reader.GetInt32("passenger_id"),
                        reader.GetString("first_name"),
                        reader.GetString("last_name")
                    );

                    // Set the FlightId property
                    passenger.FlightId = flight.FlightId;

                    // Add the Passenger object to the list
                    passengers.Add(passenger);
                }
            }

            // Close the connection
            conn.Close();



            return passengers;
        }

        // method to check in a passenger and add to the list of passengers
        private void CheckInPassenger(Passenger p)
        {
             CheckedInPassengers.Add(p);
        }

        // method to check in a luggage and add to the list of luggages
        private void CheckInLuggage(Luggage l)
        {
            LuggagesToSort.Add(l);
        }

        // method to generate a boarding pass for a passenger and luggage(s)
        private void GenerateBoardingPass(Passenger p, Luggage l)
        {
            // create a new boarding pass using flight information
            BoardingPass bp = new BoardingPass(Flight.Terminal, Flight.Gate);

            // set the boarding pass for the passenger
            p.SetBoardingPass(bp);

            // set the boarding pass for the luggage
            l.SetBoardingPass(bp);

            // add the boarding pass to the list of boarding passes
            BoardingPasses.Add(bp);
            
        }

    }
}
