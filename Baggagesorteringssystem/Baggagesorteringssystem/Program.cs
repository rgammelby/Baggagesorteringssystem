using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Baggagesorteringssystem.Business_Logic;
using Baggagesorteringssystem.Data_Access;

namespace Baggagesorteringssystem
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Airline a = new Airline();
            List<Airline> ac = a.GetAirlines("airlines");
            var aircraftCount = 0;

            foreach (Airline al in ac)
            {
                aircraftCount++;
                Console.WriteLine($"AIRLINE NUMBER {aircraftCount}\n\nAirline ID: {al.AirlineID}\nAirline Name: {al.Name}\n");
            }

            // TEsT GET FLIGHTS ON A DAY
            //string datestring = "20240602";
            //FlightsManager fm20240602 = new FlightsManager();
            //List<Flight> flights = new List<Flight>();

            //flights = fm20240602.AssignFrontDeskAndGateToFlight(datestring);

            //fm20240602.PrintFlightsInformation(flights);

            //List<string> dates = fm20240602.GetUniqueFlightDates();
            //foreach (string date in dates)
            //{
            //    Console.WriteLine(date);
            //}
            //Console.WriteLine(dates.Count);
            

            AirportManager am = new AirportManager();
            am.StartAirport();

            Console.ReadLine();
        }
    }
}