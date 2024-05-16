using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Luggage
    {
        private BoardingPass _bordingPass;

        public BoardingPass BoardingPass { get { return _bordingPass; } }

        private string _flightId;
        private string _passengerName;
        private string _destination;
        private string _flightTime;
        private double _weight;
        private bool _isCheckedIn;
        private string _qrCode { get; set; }


        List<string> qrCodeList = new List<string>();

        //constructor for luggage
        public Luggage(string flightId, string passengerName, string destination, string flightTime, double weight, bool isCheckedIn)
        {
            _flightId = flightId;
            _passengerName = passengerName;
            _destination = destination;
            _flightTime = flightTime;
            _weight = weight;
            _isCheckedIn = isCheckedIn;
            _qrCode = GenerateQRCode();
        }

        private string GenerateQRCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string qrCodeString;
            bool isUnique;

            do
            {
                StringBuilder qrCode = new StringBuilder();
                // generate a random string of 10 characters
                for (int i = 0; i < 10; i++)
                {
                    qrCode.Append(chars[random.Next(chars.Length)]);
                }

                qrCodeString = qrCode.ToString();
                isUnique = !qrCodeList.Contains(qrCodeString);

                // if the qrCode is unique, add it to the list
                if (isUnique)
                {
                    qrCodeList.Add(qrCodeString);
                }
            }while(!isUnique); //if the qrCode is not unique, generate a new one

            return qrCodeString;
        }

       

        public void SetBoardingPass(BoardingPass boardingPass)
        {
            _bordingPass = boardingPass;
        }
    }

}
