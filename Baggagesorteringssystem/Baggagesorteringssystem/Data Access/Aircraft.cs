using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Aircraft
    {
        private int _aircraftId;
        private string _aircraftModel;
        private int _totalSeats;

        public Aircraft(int aircraftId, string aircraftModel, int totalSeats)
        {
            _aircraftId = aircraftId;
            _aircraftModel = aircraftModel;
            _totalSeats = totalSeats;
        }
    }
}
