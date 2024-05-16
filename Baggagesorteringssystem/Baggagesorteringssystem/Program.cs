using System;
using System.Collections.Generic;
using System.Configuration;
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
            Connection cs = new Connection();
            cs.Connect();
        }
    }
}