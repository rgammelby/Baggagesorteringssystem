using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Airline
    {
        private int _airlineId;

        private string _name;

        private List<Airline> _airlines = new List<Airline>();

        public int AirlineID { get { return _airlineId; } }
        public string Name { get { return _name; } }

        public List<Airline> airlines { get { return _airlines; } }
        
        public Airline()
        {
            _airlines = GetAirlines("airlines");
        }

        public Airline(int id, string name)
        {
            _airlineId = id;
            _name = name;
        }

        public List<Airline> GetAirlines(string table)
        {
            Connection cs = new Connection();
            var dt = cs.RetrieveData(table);

            List<Airline> airline = new List<Airline>();

            foreach (DataRow dr in dt.Rows)
            {
                Airline a = new Airline((int)dr[0], (string)dr[1]);
                airline.Add(a);
            }

            return airline;
        }
    }
}
