using Baggagesorteringssystem.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Business_Logic
{
    /// <summary>
    /// boarding passengers from checkIn list onto the plane
    /// loading luggage onto the plane
    /// </summary>
    internal class BoardingManager
    {
        // ?private List<Flight> _flights;
        private List<Passenger> _onboardPassengers;
        private List<Luggage> _loadedLuggages;

        // Dependency Injection
        private FrontDesk _frontDesk;
        private SortingToGateSystem _sortingToGateSystem;
        public List<Luggage> LoadedLuggages { get; set; } = new List<Luggage>();

        public BoardingManager(FrontDesk frontDesk, SortingToGateSystem sortingToGateSystem)
        {
            _frontDesk = frontDesk;
            _sortingToGateSystem = sortingToGateSystem;
        }

        public void BoardPassengers()
        {
            // board passengers in CheckedInPassengers list, add them to OnboardPassengers list
            // remove them from CheckedInPassengers list

            _onboardPassengers = new List<Passenger>();

            var passenrsToBoard = new List<Passenger>(_frontDesk.CheckedInPassengers);

            foreach (var passenger in passenrsToBoard)
            {
                _onboardPassengers.Add(passenger);
                _frontDesk.CheckedInPassengers.Remove(passenger);
            }
            // after all CheckedInPassengers are on board, set gate.IsOpen = false
            //if(_frontDesk.CheckedInPassengers.Count == 0)
            {
                _frontDesk.IsOpen = false;

                // If all luggage has also been loaded, set the flight to departed
                if (_frontDesk.Flight.Gate.IsOpen == false)
                {
                    _frontDesk.Flight.IsDepartured = true;
                }
            }

        }

        /// <summary>
        /// Load luggage onto the plane
        /// </summary>
        public void LoadLuggage()
        {
            // Get luggagelist from SortingToGateSystem for this gate in this terminal
            string terminal = _frontDesk.Flight.Terminal.TerminalName;
            string teminalGateName = terminal + _frontDesk.Flight.Gate.GateName;

            List<Luggage> luggageListToload = _sortingToGateSystem._luggagByGate[teminalGateName];

            // load luggage onto the plane, then remove them from the list
            for (int i = 0; i < luggageListToload.Count; i++)
            {
                Luggage luggage = luggageListToload.First();
                LoadedLuggages.Add(luggage);
                luggageListToload.Remove(luggage);
            }

            // after all luggagByGate is loaded, set gate isOpen = false
            //if (luggageListToload.Count == 0)
            {
                _frontDesk.Flight.Gate.IsOpen = false;

                // If all passengers have also boarded, set the flight to departed
                if (_frontDesk.CheckedInPassengers.Count == 0)
                {
                    _frontDesk.Flight.IsDepartured = true;
                }
            }
        }
    }
}
