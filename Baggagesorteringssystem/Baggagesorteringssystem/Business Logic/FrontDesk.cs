using Baggagesorteringssystem.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Business_Logic
{
    internal class FrontDesk
    {
        // create a new FrontDesk object, the Passengers, Luggages, and BoardingPasses lists
        // are all initialized as empty lists, and you can start adding items to them right away.
        // list of CheckedInpassengers
        public List<Passenger> CheckedInPassengers = new List<Passenger>();

        // list of cheked-in luggages, ready to send to the sorting system
        List<Luggage> CheckedInLuggages = new List<Luggage>();

        // list of bording pass for each passenger
        List<BoardingPass> BoardingPasses = new List<BoardingPass>();

        private Flight _flight;
        private Terminal _terminal;
        private int _frontDeskId;
        public Terminal Terminal { get; set; }
        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; }
        }
        public Flight Flight { get; set; }

        // constructor for the front desk
        public FrontDesk(Terminal teminal, int frontDeskId)
        {
            terminal = teminal;
            _frontDeskId = frontDeskId;
            _isOpen = false;
        }

        // method to check in a passenger and add to the list of passengers
        public void CheckInPassenger(Passenger p)
        {
            
            
            
            CheckedInPassengers.Add(p);
        }

        // method to check in a luggage and add to the list of luggages
        public void CheckInLuggage(Luggage l)
        {
            CheckedInLuggages.Add(l);
        }

        // method to generate a boarding pass for a passenger and luggage(s)
        public void GenerateBoardingPass(Passenger p, Luggage l)
        {
            // create a new boarding pass using flight information
            BoardingPass bp = new BoardingPass(Flight.Terminal, Flight.Gate);

            // set the boarding pass for the passenger
            p.SetBoardingPass(bp);

            // set the boarding pass for the luggage
            l.SetBoardingPass(bp);

            // add the boarding pass to the list of boarding passes
            BoardingPasses.Add(bp);
            
        }


        // a start process , then in a do while loop, then in a monotor start a thread
    }
}
