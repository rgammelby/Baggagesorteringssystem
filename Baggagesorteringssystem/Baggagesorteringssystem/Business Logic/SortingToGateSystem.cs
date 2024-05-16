using Baggagesorteringssystem.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Business_Logic
{
    /// <summary>
    /// Takes in the sorted baggage and sends it to the correct gate.
    /// terminal-gate, luggage list
    /// </summary>
    internal class SortingToGateSystem
    {
        private List<Gate> _gates;
        public Dictionary<string, List<Luggage>> _luggagByGate = new Dictionary<string, List<Luggage>>();

        // ? a sortingSystem?

        public SortingToGateSystem(List<Gate> gates)
        {
            _gates = gates;
        }

        
        public void SortLuggageToGate(List<Luggage> outgoingTerminalA, List<Luggage> outgoingTerminalB)
        {
            foreach (var luggage in outgoingTerminalA)
            {
                var gateName = "A" + luggage.BoardingPass.Gate.GateName;

                // if gate does not exist in dictionary, create list for that gate
                if (!_luggagByGate.ContainsKey(gateName))
                {
                    _luggagByGate[gateName] = new List<Luggage>();
                }
                // Add luggage to gate
                _luggagByGate[gateName].Add(luggage);

                // remove luggage from outgoingTerminalA
                outgoingTerminalA.Remove(luggage);
            }

            foreach (var luggage in outgoingTerminalB)
            {
                var gateName = "B" + luggage.BoardingPass.Gate.GateName;

                // if gate does not exist in dictionary, create list for that gate
                if (!_luggagByGate.ContainsKey(gateName))
                {
                    _luggagByGate[gateName] = new List<Luggage>();
                }
                // Add luggage to the list of the same gate
                _luggagByGate[gateName].Add(luggage);

                // remove luggage from outgoingTerminalB
                outgoingTerminalB.Remove(luggage);
            }   
        }
    }
}
