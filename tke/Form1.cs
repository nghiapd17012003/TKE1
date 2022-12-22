using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTCP;


namespace tke
{
    public partial class Form1 : Form
    {
        int height = 8;
        int noOfElevator = 4;
        
        List<Button> upButtonList = new List<Button>();
        List<Button> downButtonList = new List<Button>();
        List<Elevator> elevatorList = new List<Elevator>();
        SimpleTcpServer server = new SimpleTcpServer();

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //tranh viec xung dot tai nguyen
        }


        private void upButton(Panel p)
        {
            for (int i = 0; i < height; i++)
            {
                Button singleUp = new Button();
                singleUp.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\upArrow.png");
                singleUp.Location = new Point(0, 40 * i);
                singleUp.Size = new Size(37, 30);
                if (i == 0)
                {
                    singleUp.Visible = false;
                }

                p.Controls.Add(singleUp);
                upButtonList.Add(singleUp);
            }
        }

        private void downButton(Panel p)
        {
            for (int i = 0; i < height; i++)
            {
                Button singleDown = new Button();
                singleDown.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\downArrow.png");
                singleDown.Location = new Point(47, 40 * i);
                singleDown.Size = new Size(37, 30);
                if (i == height - 1)
                {
                    singleDown.Visible = false;
                }
                p.Controls.Add(singleDown);
                downButtonList.Add(singleDown);
            }
        }

        private void levelLable(Panel p)
        {
            for (int i = 0; i < height; i++)
            {
                Label level = new Label();
                level.Text = "" + (height -i);
                level.Location = new Point(94, 40 * i);
                level.Size = new Size(37, 30);
                level.TextAlign = (System.Drawing.ContentAlignment)HorizontalAlignment.Center;
                level.TextAlign = (System.Drawing.ContentAlignment)System.Windows.Forms.VisualStyles.VerticalAlignment.Bottom;
               
                
                p.Controls.Add(level);              
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
            levelLable(controlPanel);

            Controls.Add(controlPanel);
        }

        public void positionSet()
        {
            
        }
        
        public void newElevator(int elevatorNo)
        {
            Elevator elevator = new Elevator();
            Panel elevatorPanel = new Panel();
            int elevatorePanelHeight = height * 40;
            if (elevatorePanelHeight < 270)
            {
                elevatorePanelHeight += 270;//make sure if the height is <= 2, there will be enough space for other components
            }
            elevatorPanel.Size = new Size(300, elevatorePanelHeight);
            elevatorPanel.Location = new Point(240 + elevatorNo * 320, 0);//control panel width + control panel left = 200 + 20 = 220, elevator panel width + space between each panel = 300 + 20 = 320;
            elevatorPanel.BorderStyle = BorderStyle.Fixed3D;

            
            elevator.positionScreen.Location = new Point(94, 0);
            elevator.positionScreen.Size = new Size(50, 50);
            elevator.positionScreen.BackColor = Color.Black;
            elevator.positionScreen.Text = "1";
            elevator.positionScreen.Font = new Font("Broadway", 12);
            elevator.positionScreen.ForeColor = Color.White;
            elevatorPanel.Controls.Add(elevator.positionScreen);

            elevator.directionScreen.Location = new Point(144, 0);          
            elevator.directionScreen.Size = new Size(50, 50);
            elevator.directionScreen.BackColor = Color.Black;
            //elevator.directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\upArrowInWhite.png");
            elevatorPanel.Controls.Add(elevator.directionScreen);

            elevator.elevatorDoor.Location = new Point(94, 60);
            elevator.elevatorDoor.Size = new Size(100, 140);
            elevator.elevatorDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\closeDoor.png");
            elevatorPanel.Controls.Add(elevator.elevatorDoor);

            elevator.closeDoor.Location = new Point(244, 60);
            elevator.closeDoor.Size = new Size(50, 50);
            elevator.closeDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\closeDoorButton.png");
            elevatorPanel.Controls.Add(elevator.closeDoor);

            elevator.openDoor.Location = new Point(194, 60);
            elevator.openDoor.Size = new Size(50, 50);
            elevator.openDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\openDoorButton.png");
            elevatorPanel.Controls.Add(elevator.openDoor);

            elevator.status.Location = new Point(244, 0);
            elevator.status.Size = new Size(50, 50);
            elevator.status.Text = "Stop";
            elevator.status.BackColor = Color.Black;
            elevator.status.ForeColor = Color.White;
            elevatorPanel.Controls.Add(elevator.status);

            elevator.mode.Location = new Point(194, 0);
            elevator.mode.Size = new Size(50, 50);
            elevator.mode.Font = new Font("Broadway", 8);
            //elevator.mode.Text = "Priority";
            elevator.mode.BackColor = Color.Black;
            elevatorPanel.Controls.Add(elevator.mode);

            elevator.connectingStatus.Location = new Point(194, 120);
            elevator.connectingStatus.Size = new Size(100, 80);
            elevator.connectingStatus.Text = "Disconected";
            elevator.connectingStatus.BackColor = Color.White;
            elevatorPanel.Controls.Add(elevator.connectingStatus);

            elevator.eventCapturer.Location = new Point(94, 200);
            elevator.eventCapturer.Size = new Size(200, 100);
            elevator.eventCapturer.Multiline = true;
            elevator.eventCapturer.ScrollBars = ScrollBars.Both;
            elevator.eventCapturer.Text = "Status:";
            elevatorPanel.Controls.Add(elevator.eventCapturer);

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
                cabin.Visible = false;
                elevator.cabinPosition.Add(cabin);
                elevatorPanel.Controls.Add(cabin);

            }
            elevatorList.Add(elevator);
            
            Controls.Add(elevatorPanel);
        }

        private void ClientConnected(Object sender,TcpClient e)
        {
            
        }

        private void Disconnected(Object sender, TcpClient e)
        { 


        }

        private void DataReceived(Object sender, SimpleTCP.Message e)
        {
           
            Byte[] str = Encoding.GetEncoding(28591).GetBytes(e.MessageString);

            var Convertdata = BitConverter.ToString(str);
            
            //postion
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                {      
                    txt1.Text = str[i + 6].ToString();
                    elevatorList[0].positionScreen.Text = str[i + 6].ToString();
                    elevatorList[0].cabinPosition[height - Convert.ToInt32(str[i + 6])].BackColor = Color.Blue;//height - current possition because the cabin buttons have been added from top to bottom
                    elevatorList[0].cabinPosition[height - Convert.ToInt32(str[i + 6])].Visible = true;
                    if (str[i + 6] == 0x01)
                    {
                        elevatorList[0].cabinPosition[height - Convert.ToInt32(str[i + 6]) - 1].Visible = false;
                    }

                    else if (Convert.ToInt32(str[i + 6]) == height)
                    {
                        elevatorList[0].cabinPosition[1].Visible = false;
                    }

                    else 
                    {
                        elevatorList[0].cabinPosition[height - Convert.ToInt32(str[i + 6]) - 1].Visible = false;
                        elevatorList[0].cabinPosition[height - Convert.ToInt32(str[i + 6]) + 1].Visible = false;
                    }
                }
            }

            //direction
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                {
                    if (str[i + 5] == 0x11)
                    {
                        elevatorList[0].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\upArrowInWhite.png");
                    }

                    if (str[i + 5] == 0x21)
                    {
                        elevatorList[0].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\downArrowInWhite.png");
                    }

                    if (str[i + 5] == 0x00)
                    {
                        elevatorList[0].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\black-color.png");
                    }
                }
            }

            //mode
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x48 && str[i + 5] == 0xFF)
                {
                    if (str[i + 6] == 0x40)
                    {
                        elevatorList[0].mode.Text = "Fire!";
                        elevatorList[0].mode.BackColor = Color.Yellow;
                        elevatorList[0].mode.ForeColor = Color.Red;
                    }
                    else if (str[i + 6] == 0x00)
                    {
                        elevatorList[0].mode.Text = "";
                        elevatorList[0].mode.BackColor = Color.Black;
                    }

                    else if (str[i + 6] == 0x20)
                    {
                        elevatorList[0].mode.Text = "Priority";
                        elevatorList[0].mode.BackColor = Color.Yellow;
                        elevatorList[0].mode.ForeColor = Color.Red;
                    }

                    else if (str[i + 6] == 0x80)
                    {
                        elevatorList[0].mode.Text = "Full Load!";
                        elevatorList[0].mode.BackColor = Color.Yellow;
                        elevatorList[0].mode.ForeColor = Color.Red;
                    }

                    else if (str[i + 6] == 0x90)
                    {
                        elevatorList[0].mode.Text = "Over Load!";
                        elevatorList[0].mode.BackColor = Color.Yellow;
                        elevatorList[0].mode.ForeColor = Color.Red;
                    }

                    else if (str[i + 6] == 0x08)
                    {
                        elevatorList[0].mode.Text = "Out Of Order!";
                        elevatorList[0].mode.BackColor = Color.Yellow;
                        elevatorList[0].mode.ForeColor = Color.Red;
                    }

                    
                }

                else if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                {
                    if (str[i + 8] == 0x94)
                    {
                        elevatorList[0].mode.Text = "JU";
                        elevatorList[0].mode.BackColor = Color.Black;
                        elevatorList[0].mode.ForeColor = Color.Red;
                    }
                    else if (str[i + 8] == 0x92)
                    {
                        elevatorList[0].mode.Text = "IF";
                        elevatorList[0].mode.BackColor = Color.Yellow;
                        elevatorList[0].mode.ForeColor = Color.Red;
                    }
                }
            }
            
                
        }

        private void DataReveivedEle(String data)
        {
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            IPAddress ipHost = IPAddress.Parse("192.168.0.2");
            int tempport = 5002;

            server = new SimpleTcpServer();
            server.StringEncoder = Encoding.GetEncoding(28591);//Để nhận được các dữ liệu byte lớn hơn 7F
            server.Start(ipHost, tempport);
            server.ClientConnected += ClientConnected;
            server.ClientDisconnected += Disconnected;
            server.DataReceived += DataReceived;//bat du lieu lien tuc tu thang

            

            
            addControlPanel();
            
            for (int i = 0; i < noOfElevator; i++)
            {
                newElevator(i);
            }
        }
    }
}
