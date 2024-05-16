using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Gate
    {
        private Terminal _terminal;
        private int _gateId;
        private string _gateName;

        public Gate(Terminal terminal, int gateId, string gateName)
        {
            _terminal = terminal;
            _gateId = gateId;
            _gateName = gateName;
        }
    }
}
