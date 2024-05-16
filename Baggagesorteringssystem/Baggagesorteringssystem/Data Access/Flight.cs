using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Flight
    {
        private int _flightId;
        private int _flightNumber;
        private (byte, byte) _departureTime;
        private (byte, byte) _arrivalTime;
        private int _airlineId;
        private int _aircraftId;
        private int _destinationId;

        public Flight(int flightId, int flightNumber, (byte, byte) departureTime, (byte, byte) arrivalTime, int airlineId, int aircraftId, int destinationId)
        {
            _flightId = flightId;
            _flightNumber = flightNumber;
            _departureTime = departureTime;
            _arrivalTime = arrivalTime;
            _airlineId = airlineId;
            _aircraftId = aircraftId;
            _destinationId = destinationId;
        }
    }
}
