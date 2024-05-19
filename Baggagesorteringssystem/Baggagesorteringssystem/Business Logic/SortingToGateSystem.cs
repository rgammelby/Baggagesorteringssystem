using Baggagesorteringssystem.Data_Access;
using System;
using System.Collections.Concurrent;
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

         public List<Gate> Gates { get; set; }

        public SortingToGateSystem()
        {
            
        }

        
        private object _lock = new object();

        public void SortLuggageToGate(ConcurrentQueue<Luggage> outgoingTerminalA, ConcurrentQueue<Luggage> outgoingTerminalB)
        {
            lock (_lock)
            {


                while ( outgoingTerminalA.TryDequeue(out Luggage luggageA))
                {
                    var gateName = "A" + luggageA.BoardingPass.Gate.GateName;

                    // if gate does not exist in dictionary, create list for that gate
                    if (!_luggagByGate.ContainsKey(gateName))
                    {
                        _luggagByGate[gateName] = new List<Luggage>();
                    }
                    // Add luggage to gate
                    _luggagByGate[gateName].Add(luggageA);
                }

                while (outgoingTerminalB.TryDequeue(out Luggage luggageB))
                {
                    var gateName = "B" + luggageB.BoardingPass.Gate.GateName;

                    // if gate does not exist in dictionary, create list for that gate
                    if (!_luggagByGate.ContainsKey(gateName))
                    {
                        _luggagByGate[gateName] = new List<Luggage>();
                    }
                    // Add luggage to gate
                    _luggagByGate[gateName].Add(luggageB);
                }

                //List<Luggage> itemsToRemoveA = new List<Luggage>();
                //List<Luggage> itemsToRemoveB = new List<Luggage>();


                //foreach (var luggage in outgoingTerminalA)
                //{
                //    var gateName = "A" + luggage.BoardingPass.Gate.GateName;

                //    // if gate does not exist in dictionary, create list for that gate
                //    if (!_luggagByGate.ContainsKey(gateName))
                //    {
                //        _luggagByGate[gateName] = new List<Luggage>();
                //    }
                //    // Add luggage to gate
                //    _luggagByGate[gateName].Add(luggage);

                //    // instead of removing the item here, add it to the itemsToRemoveA list
                //    itemsToRemoveA.Add(luggage);
                //}

                //foreach (var luggage in outgoingTerminalB)
                //{
                //    var gateName = "B" + luggage.BoardingPass.Gate.GateName;

                //    // if gate does not exist in dictionary, create list for that gate
                //    if (!_luggagByGate.ContainsKey(gateName))
                //    {
                //        _luggagByGate[gateName] = new List<Luggage>();
                //    }
                //    // Add luggage to the list of the same gate
                //    _luggagByGate[gateName].Add(luggage);

                //    // instead of removing the item here, add it to the itemsToRemoveB list
                //    itemsToRemoveB.Add(luggage);
                //}

                //// now remove the items from outgoingTerminalA
                //foreach (var luggage in itemsToRemoveA)
                //{
                //    outgoingTerminalA.Remove(luggage);
                //}

                //// and remove the items from outgoingTerminalB
                //foreach (var luggage in itemsToRemoveB)
                //{
                //    outgoingTerminalB.Remove(luggage);
                //}
            }
        }
    }
}
