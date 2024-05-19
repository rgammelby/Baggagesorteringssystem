using Baggagesorteringssystem.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace Baggagesorteringssystem.Business_Logic
{
    internal class AirportManager
    {
        //private Connection _cs = new Connection();
        private FlightsManager _flightsManager;

        public AirportManager()
        {
            _flightsManager = new FlightsManager();
        }

        

        public void StartAirport()
        {
            // Get all dates in the database
            List<string> dates = _flightsManager.GetUniqueFlightDates();

            // create a dictionary with the date string as key and a list of flights on the date as value
            Dictionary<string, List<Flight>> flightsByDate = new Dictionary<string, List<Flight>>();

            //while (true)
            //{
                
            //}
            foreach (string date in dates)
            {
                // get all flights on the date
                List<Flight> flightsOfTheDay = _flightsManager.AssignFrontDeskAndGateToFlight(date);

                // Log the numbers of flights on the date
                Console.WriteLine($"Date: {date}, Number of Flights: {flightsOfTheDay.Count}");

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
