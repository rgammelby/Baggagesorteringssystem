using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class BoardingPass
    {
        private Terminal _terminal;
        private Gate _gate;
        private int _boardingPassNumber;

        public Terminal Terminal { get { return _terminal; } }
        public Gate Gate { get { return _gate; } }
        public int BoardingPassNumber { get { return _boardingPassNumber; } }

        List<int> BoardingPassNumbers = new List<int>();

        private int GenerateBoardingPassNumber()
        {
            Random r = new Random();
            var boardingPassNumber = r.Next(9999, 99999);

            BoardingPassNumbers.Add(boardingPassNumber);

            if (BoardingPassNumbers.Contains(boardingPassNumber))
                boardingPassNumber = GenerateBoardingPassNumber();

            return boardingPassNumber;
        }

        public List<BoardingPass> GetBoardingPasses(string table)
        {
            private List<Aircraft> GetAircraft(string table)
            {
                
            }
        }

        public BoardingPass(Terminal terminal, Gate gate)
        {
            _terminal = terminal;
            _gate = gate;
            _boardingPassNumber = GenerateBoardingPassNumber();
        }
    }
}
