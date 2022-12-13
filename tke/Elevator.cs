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
        public Button positionScreen;
        public Button directionScreen;
        public Button elevatorDoor;
        public Button closeDoor;
        public Button openDoor;
        public Button status;
        public Button mode;
        public Button connectingStatus;
        public TextBox eventCapturer;

        public Elevator()
        {
            insideEleButton = new List<Button>();
            cabinPosition = new List<Button>();
            positionScreen = new Button();
            directionScreen = new Button();
            elevatorDoor = new Button();
            closeDoor = new Button();
            openDoor = new Button();
            status = new Button();
            mode = new Button();
            connectingStatus = new Button();
            eventCapturer = new TextBox();
        }      
    }
}
