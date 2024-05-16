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

         // Add properties that will be generated later based on flight plan/ flightManager 
         // set the values later when known
         private Terminal _terminal; // Added
         private Gate _gate; // Added
         private List<FrontDesk> _frontDesks; // Added
         public Terminal Terminal
         {
             get { return _terminal; }
             set { _terminal = value; }
         }
        
         public Gate Gate
         {
             get { return _gate; }
             set { _gate = value; }
         }
         public List<FrontDesk> FrontDesks // Added
         {
             get { return _frontDesks; }
             set { _frontDesks = value; }
         }


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
