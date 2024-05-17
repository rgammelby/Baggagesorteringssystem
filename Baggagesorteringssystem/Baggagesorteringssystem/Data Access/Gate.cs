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
        
        // show gate is open or closed
        private bool _isOpen;

        public string GateName { get; private set; }
         /* use properties to expose private fields. The IsOpen property's 
  * a public property that gets and sets the value of the private field _isOpen.
  *  whenever you create a new Gate object, _isOpen will be initialized to false. 
  *  You can then use the IsOpen property to get or set the value of _isOpen from outside the class:
  *  you still need the _isOpen field. The IsOpen property is a way to control access to the _isOpen field
  *  from outside the Gate class.
  *  When you write gate.IsOpen = true; in another class, 
  *  what you're actually doing is calling the set accessor of the IsOpen property, 
  *  which then sets the value of the _isOpen field to true.
  *  Similarly, when you write bool isOpen = gate.IsOpen; 
  *  in another class, you're calling the get accessor of the IsOpen property, which returns the value of the _isOpen field.
  *  So, the _isOpen field is where the actual data is stored, 
  *  and the IsOpen property is how you access that data from outside the Gate class. 
  *  This is a common pattern in C# and other object-oriented programming languages, known as encapsulation. 
  *  It allows you to control how the fields of a class are accessed and modified.
  * */
 public bool IsOpen
 {
     get { return _isOpen; }
     set { _isOpen = value; }
 }

        public Gate(Terminal terminal, int gateId, string gateName)
        {
            _terminal = terminal;
            _gateId = gateId;
            GateName = gateName;
            _isOpen = false;
        }
    }
}
