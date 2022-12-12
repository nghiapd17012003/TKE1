using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tke
{
    internal class Elevator
    {
        public List<Button> insideEleButton;
        public List<Button> cabinPosition;

        public Elevator()
        {
            insideEleButton = new List<Button>();
            cabinPosition = new List<Button>();
        }

        
    }
}
