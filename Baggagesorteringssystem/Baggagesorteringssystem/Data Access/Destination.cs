using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Destination
    {
        private int _destinationId;
        private string _city;
        private string _country;
        private string _airportCode;

        public Destination(int destinationId, string city, string country, string airportCode)
        {
            _destinationId = destinationId;
            _city = city;
            _country = country;
            _airportCode = airportCode;
        }
    }
}
