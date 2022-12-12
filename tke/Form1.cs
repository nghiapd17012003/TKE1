using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace tke
{
    public partial class Form1 : Form
    {
        int height = 8;
        int noOfElevator = 4;
        List<Button> upButtonList = new List<Button>();
        List<Button> downButtonList = new List<Button>();
        List<Button> positionScreen = new List<Button>();
        List<Elevator> elevatorList = new List<Elevator>();

        public Form1()
        {
            InitializeComponent();
        }


        private void upButton(Panel p)
        {
            for (int i = 0; i < height; i++)
            {
                Button singleUp = new Button();
                singleUp.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE\tke\image\upArrow.png");
                singleUp.Location = new Point(0, 40 * i); 
                singleUp.Size = new Size(37, 30);
                p.Controls.Add(singleUp);   
                upButtonList.Add(singleUp);
            }
        }

        private void downButton(Panel p)
        {
            for (int i = 0; i < height; i++)
            {
                Button singleDown = new Button();
                singleDown.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE\tke\image\downArrow.png");
                singleDown.Location = new Point(47, 40 * i);
                singleDown.Size = new Size(37, 30);
                p.Controls.Add(singleDown);
                downButtonList.Add(singleDown);
            }
        }

        public void addControlPanel()
        {
            Panel controlPanel = new Panel();
            controlPanel.Size = new Size(200, height * 40);
            controlPanel.Location = new Point(0, 0);
            controlPanel.BorderStyle = BorderStyle.FixedSingle;
            upButton(controlPanel);
            downButton(controlPanel);

            Controls.Add(controlPanel);
        }
        
        public void newElevator(int elevatorNo)
        {
            Elevator elevator = new Elevator();
            Panel elevatorPanel = new Panel();
            int elevatorePanelHeight = height * 40;
            if (elevatorePanelHeight < 200)
            {
                elevatorePanelHeight += 200;//make sure if the height is <= 2, there will be enough space for other components
            }
            elevatorPanel.Size = new Size(250, elevatorePanelHeight);
            elevatorPanel.Location = new Point(240 + elevatorNo * 270, 0);//control panel width + control panel left = 200 + 20 = 220, elevator panel width + space between each panel = 250 + 20 = 270;
            elevatorPanel.BorderStyle = BorderStyle.Fixed3D;

            Button positionScreen = new Button();
            positionScreen.Location = new Point(94, 0);
            positionScreen.Size = new Size(100, 50);
            positionScreen.BackColor = Color.Black;
            positionScreen.Text = "1";
            positionScreen.ForeColor = Color.White;
            elevatorPanel.Controls.Add(positionScreen);

            Button directionScreen = new Button();
            directionScreen.Location = new Point(194, 0);
            directionScreen.Size = new Size(50, 50);
            directionScreen.BackColor = Color.Black;
            directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE\tke\image\upArrowInWhite.png");
            elevatorPanel.Controls.Add(directionScreen);

            Button elevatorDoor = new Button();
            elevatorDoor.Location = new Point(94, 60);
            elevatorDoor.Size = new Size(100, 140);
            elevatorDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE\tke\image\closeDoor.png");
            elevatorPanel.Controls.Add(elevatorDoor);

            for (int i = 0; i < height; i++)
            {
                Button numberButton = new Button();
                numberButton.Location = new Point(0,  i * 40);//x-coordinate = 0 because the Location method count from each panel, not the top left corner of the client! y-distance between button + y-button size = 10 + 30 = 40
                numberButton.Text = "" + (height - i);
                numberButton.Size = new Size(37, 30);    
                elevator.insideEleButton.Add(numberButton);
                elevatorPanel.Controls.Add(numberButton);

                Button cabin = new Button();
                cabin.BackColor = Color.Blue;
                cabin.Location = new Point(47, i * 40);//button x-size + distance between button and cabin = 37 + 10 = 47;
                cabin.Size = new Size(37, 30);
                elevator.cabinPosition.Add(cabin);
                elevatorPanel.Controls.Add(cabin);

            }
            elevatorList.Add(elevator);
            
            Controls.Add(elevatorPanel);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            addControlPanel();

            for (int i = 0; i < noOfElevator; i++)
            {
                newElevator(i);
            }
        }


    }
}
