using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Passenger
    {
        private int _passengerId;
        private string _passengerFistName;
        private string _passengerLastName;
        private int _passportNumber;
        private int _flightId;
        private BoardingPass _boardingPass;

        public int FlightId { get; set; }
        public int PassengerId { get { return _passengerId; } }

        List<int> PassportNumbers = new List<int>();

        private int GeneratePassportNumber()
        {
            Random r = new Random();

            //var passportNumber = r.Next(9999999, 99999999);

            //PassportNumbers.Add(passportNumber);

            //if (PassportNumbers.Contains(passportNumber))
            //    passportNumber = GeneratePassportNumber();

            //return passportNumber;

            int passportNumber;
            do
            {
                passportNumber = r.Next(9999999, 99999999);
            } while (PassportNumbers.Contains(passportNumber));

            PassportNumbers.Add(passportNumber);
            return passportNumber;
        }

        public void SetBoardingPass(BoardingPass bp)
        {
            this._boardingPass = bp;
        }

        public Passenger(int id, string firstName, string lastName)
        {
            _passengerId = id;
            _passengerFistName = firstName;
            _passengerLastName = lastName;
            _passportNumber = GeneratePassportNumber();
        }
    }
}
