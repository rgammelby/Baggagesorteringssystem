using System;
using System.Collections.Generic;
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
            
        }
    }

<<<<<<< HEAD
    

    class Passenger
    {
        private 
=======
    class Time
    {
        public (byte, byte) Timestamp;

        public const int MS = 100;
        public const byte MINUTES = 60;
        public const byte HOURS = 24;

        public void SetTime()
        {
            Timestamp.Item1 = 0;  // Hours
            Timestamp.Item2 = 0;  // Minutes

            while (true)
            {
                Thread.Sleep(MS);
                Timestamp.Item2++;
                if (Timestamp.Item2 == MINUTES)  // Loop minutes
                {
                    Timestamp.Item1++;
                    Timestamp.Item2 = 0;

                    if (Timestamp.Item1 == HOURS)  // Loop hours
                        Timestamp.Item1 = 0;
                }
            }
        }

        public (byte, byte) GetTimestamp()
        {
            return (Timestamp.Item1, Timestamp.Item2);
        }

        public Time()
        {
            Thread t = new Thread(new ThreadStart(SetTime));
            t.Start();
        }
>>>>>>> 70bf2deebda43a59f56b3ea5a08ee2d696777214
    }
}