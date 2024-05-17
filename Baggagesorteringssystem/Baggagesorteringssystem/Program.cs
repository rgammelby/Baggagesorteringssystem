using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        }
    }
}