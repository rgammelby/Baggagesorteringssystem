using Baggagesorteringssystem.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using ZstdSharp.Unsafe;
using Google.Protobuf;
using System.Threading;

namespace Baggagesorteringssystem.Business_Logic
{
    internal class AirportManager
    {
        //private Connection _cs = new Connection();
        private FlightsManager _flightsManager;
        private FrontDesk _frontDesk;
        private Gate _gate;
        private SortingSystem _sortingSystem;
        private SortingToGateSystem _sortingToGateSystem;
        private BoardingManager _boardingManager;

        public AirportManager()
        {
            _flightsManager = new FlightsManager();
            _sortingSystem = new SortingSystem();
            _sortingToGateSystem = new SortingToGateSystem();
        }


        private object _lock = new object();

        public async Task StartAirport()
        {
            // Get all dates in the database
            List<string> dates = _flightsManager.GetUniqueFlightDates();

            // create a dictionary with the date string as key and a list of flights on the date as value
            Dictionary<string, List<Flight>> flightsByDate = new Dictionary<string, List<Flight>>();

            foreach (string date in dates)
            {


                // get all flights on the date
                List<Flight> flightsOfTheDay = _flightsManager.GetFlightsByDate(date);

                var flightsOfTheDayCopy = new List<Flight>(flightsOfTheDay);


                // Log the numbers of flights on the date
                Console.WriteLine($"Date: {date}, Number of Flights: {flightsOfTheDay.Count}");

                while (flightsOfTheDayCopy.Any())
                {
                    //List<Flight> flightsOfTheDay = _flightsManager.AssignFrontDeskAndGateToFlightThread(date);
                    _flightsManager.AssignFrontDeskAndGateToFlight(flightsOfTheDayCopy);
                    

                    var flightTasks = new List<Task>();

                    // foreach flight, create a FrontDesk, then satart ChcekInPassengersAnfLuggageThread(Flight flight)
                    foreach (Flight flight in flightsOfTheDayCopy.Where(x => x.Gate != null && x.FrontDesk != null).OrderBy(x => x.DepartureTime).ToList())
                    {
                        var task = Task.Run(async () =>
                        {

                            var _frontDesk = flight.FrontDesk; // new FrontDesk(flight.Terminal, flight.FrontDesk.FrontDeskId);
                            //_frontDesk.CheckInPassengersAndLuggageThread();
                            _frontDesk.CheckInPassengersAndLuggage();

                            await Task.Delay(1000);

                            // sorting luggages to terminal
                            _sortingSystem.AddLuggage(_frontDesk.LuggagesToSort);
                            _frontDesk.LuggagesToSort.Clear();

                            // sort luggage to gate
                            _sortingToGateSystem.SortLuggageToGate(_sortingSystem.OutgoingTerminalA, _sortingSystem.OutgoingTerminalB);

                            // get checkedInPassengers List, board passengers and load luggage
                            var _boardingManager = new BoardingManager(_frontDesk, _sortingToGateSystem);
                            _boardingManager.BoardPassengers();
                            _boardingManager.LoadLuggage();

                            lock (_lock)
                            {
                                flightsOfTheDayCopy.Remove(flight);
                            }

                        });
                        flightTasks.Add(task);
                    }



                    await Task.WhenAll(flightTasks);

                    Console.WriteLine("Flights remaining: " + flightsOfTheDayCopy.Count());

                }

                //  Plane info showing screen 


                // log to status report, date, number of flights, flights, numbers of passengers




                //Add the list of flights to the dictionary
                flightsByDate.Add(date, flightsOfTheDay);

                // logging:
                // Create a dictionary to hold the log data
                var logData = new Dictionary<string, object>
                {
                    { "Date", date },
                    { "Number of Flights", flightsOfTheDay.Count },
                    { "Flights", flightsOfTheDay }
                };

                // Serialize the dictionary to a JSON string
                string json = JsonConvert.SerializeObject(logData, Newtonsoft.Json.Formatting.Indented);

                // Append the JSON string to a file, create the file if it does not exist
                File.AppendAllText(@"C:\Users\twan\source\repos\log.json", json);

               

            }
        }
    }
}
