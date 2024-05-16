using Baggagesorteringssystem.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Business_Logic
{
    internal class SortingSystem
    {
        // all incoming luggage is placed in LuggageToSort list for sorting
        List<Luggage> LuggageToSort = new List<Luggage>();

        // for luggage with no designated terminal
        List<Luggage> LostLuggage = new List<Luggage>();

        // mayb this instead of sorted luggage list
        List<Luggage> OutgoingTerminalA = new List<Luggage>();
        List<Luggage> OutgoingTerminalB = new List<Luggage>();

        // luggage as parameter to check props
        public void SortLuggage(Luggage l)
        {
            if (l.BoardingPass.Terminal.TerminalName == "A")
            {
                // send to correct terminal
                // or add to outgoing terminal A list
                OutgoingTerminalA.Add(l);
            }

            else if (l.BoardingPass.Terminal.TerminalName == "B")
            {
                // send to other terminal
                // or add to outgoing terminal B list
                OutgoingTerminalB.Add(l);
            }

            else
            {
                // no designated terminal on boarding pass
                LostLuggage.Add(l);
            }
        }

        // to run continuously on a thread; constantly sorts incoming luggage
        public void SendLuggageToSortingFacility()
        {
            while(true)
            {
                while(LuggageToSort.Count > 0)
                {
                    var l = LuggageToSort.Last();
                    SortLuggage(l);

                    LuggageToSort.Remove(LuggageToSort.Last());
                }
            }
        }
    }
}
