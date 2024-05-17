using System;
using System.Collections.Generic;
using System.Data;
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
        private List<Aircraft> _aircraftList = new List<Aircraft>();

        public int AircraftId { get { return _aircraftId;} }
        public string AircraftModel {  get { return _aircraftModel; } }
        public int TotalSeats {  get { return _totalSeats; } }
        public List<Aircraft> AircraftList {  get { return _aircraftList; } }   

        public Aircraft(int aircraftId, string aircraftModel, int totalSeats)
        {
            _aircraftId = aircraftId;
            _aircraftModel = aircraftModel;
            _totalSeats = totalSeats;
            _aircraftList = GetAircraft("aircrafts");
        }

        private List<Aircraft> GetAircraft(string table)
        {
            Connection cs = new Connection();
            var dt = cs.RetrieveData(table);

            List<Aircraft> aircraft = new List<Aircraft>();

            foreach (DataColumn col in dt.Columns)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Aircraft a = new Aircraft((int)dr[0], (string)dr[1], (int)dr[2]);
                    aircraft.Add(a);
                }
            }

            return aircraft;
        }
    }
}
