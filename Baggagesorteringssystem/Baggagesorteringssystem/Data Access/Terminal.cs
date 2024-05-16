using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baggagesorteringssystem.Data_Access
{
    internal class Terminal
    {
        private int _terminalId;
        private string _terminalName;

        public string TerminalName { get { return _terminalName; } }

        public Terminal(int terminalId, string terminalName)
        {
            _terminalId = terminalId;
            _terminalName = terminalName;
        }
    }
}
