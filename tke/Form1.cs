using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
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
                level.Text = "" + (height - i);
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
            //elevator.positionScreen.Text = "1";
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
            // elevator.elevatorDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\closeDoor.png");
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
                numberButton.Location = new Point(0, i * 40);//x-coordinate = 0 because the Location method count from each panel, not the top left corner of the client! y-distance between button + y-button size = 10 + 30 = 40
                numberButton.Text = "" + (height - i);
                numberButton.Size = new Size(37, 30);
                elevator.insideEleButton.Add(numberButton);
                elevatorPanel.Controls.Add(numberButton);

                Button cabin = new Button();
                cabin.BackColor = Color.Black;
                cabin.Location = new Point(47, i * 40);//button x-size + distance between button and cabin = 37 + 10 = 47;
                cabin.Size = new Size(37, 30);
                elevator.cabinPosition.Add(cabin);
                elevatorPanel.Controls.Add(cabin);

            }
            elevatorList.Add(elevator);

            Controls.Add(elevatorPanel);
        }

        private void ClientConnected(Object sender, TcpClient e)
        {

        }

        private void Disconnected(Object sender, TcpClient e)
        {


        }

        private void DataReceived(Object sender, SimpleTCP.Message e)
        {
            int a = 0;
            Byte[] str = Encoding.GetEncoding(28591).GetBytes(e.MessageString);

            var Convertdata = BitConverter.ToString(str);

            //postion
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                    {

                        elevatorList[a].positionScreen.Text = str[i + 6].ToString();
                        //elevatorList[0].cabinPosition[height - Convert.ToInt32(str[i + 6])].BackColor = Color.Blue;//height - current possition because the cabin buttons have been added from top to bottom


                        for (int j = 0; j < height; j++)
                        {
                            if (height - j == Convert.ToInt32(str[i + 6]))
                            {
                                elevatorList[a].cabinPosition[j].Visible = true;
                            }

                            else
                            {
                                elevatorList[a].cabinPosition[j].Visible = false;
                            }
                        }
                    }
                }

                catch (IndexOutOfRangeException ea)
                {
                    Console.WriteLine(ea.Message);
                }

            }

            //direction
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                {
                    if (str[i + 5] == 0x11)
                    {
                        elevatorList[a].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\upArrowInWhite.png");
                    }

                    if (str[i + 5] == 0x21)
                    {
                        elevatorList[a].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\downArrowInWhite.png");
                    }

                    if (str[i + 5] == 0x00)
                    {
                        elevatorList[a].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\black-color.png");
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
                        elevatorList[a].mode.Text = "Fire!";
                        elevatorList[a].mode.BackColor = Color.Yellow;
                        elevatorList[a].mode.ForeColor = Color.Red;
                    }
                    else if (str[i + 6] == 0x00)
                    {
                        elevatorList[a].mode.Text = "";
                        elevatorList[a].mode.BackColor = Color.Black;
                    }

                    else if (str[i + 6] == 0x20)
                    {
                        elevatorList[a].mode.Text = "Priority";
                        elevatorList[a].mode.BackColor = Color.Yellow;
                        elevatorList[a].mode.ForeColor = Color.Red;
                    }

                    else if (str[i + 6] == 0x80)
                    {
                        elevatorList[a].mode.Text = "Full Load!";
                        elevatorList[a].mode.BackColor = Color.Yellow;
                        elevatorList[a].mode.ForeColor = Color.Red;
                    }

                    else if (str[i + 6] == 0x90)
                    {
                        elevatorList[a].mode.Text = "Over Load!";
                        elevatorList[a].mode.BackColor = Color.Yellow;
                        elevatorList[a].mode.ForeColor = Color.Red;
                    }

                    else if (str[i + 6] == 0x08)
                    {
                        elevatorList[a].mode.Text = "Out Of Order!";
                        elevatorList[a].mode.BackColor = Color.Yellow;
                        elevatorList[a].mode.ForeColor = Color.Red;
                    }


                }

                else if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                {
                    if (str[i + 8] == 0x94)
                    {
                        elevatorList[a].mode.Text = "JU";
                        elevatorList[a].mode.BackColor = Color.Black;
                        elevatorList[a].mode.ForeColor = Color.Red;
                    }
                    else if (str[i + 8] == 0x92)
                    {
                        elevatorList[a].mode.Text = "IF";
                        elevatorList[a].mode.BackColor = Color.Yellow;
                        elevatorList[a].mode.ForeColor = Color.Red;
                    }
                }
            }

            //elevatorDoor
            for (int i = 0; i < str.Length; i++)
            {
                try
                {

                    if (str[i] == 0x01 && str[i + 1] == 0x00 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x73 && str[i + 5] == 0xFF)
                    {

                        if (str[i + 6] == 0x08)
                        {
                            elevatorList[a].elevatorDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\closeDoor.png");
                            for (int j = 0; j < height; j++)
                            {
                                elevatorList[a].cabinPosition[j].Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\closeDoorMini.png");
                            }
                        }

                        else if (str[i + 6] == 0x04)
                        {
                            elevatorList[a].elevatorDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\openDoor.png");
                            for (int j = 0; j < height; j++)
                            {
                                elevatorList[a].cabinPosition[j].Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\openDoorMini.png");
                            }
                        }
                    }
                }

                catch (IndexOutOfRangeException ea)
                {
                    Console.WriteLine(ea.Message);
                }


            }

            //light
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    if (str[i] == 0x01 && str[i + 1] == 0x00 && str[i + 2] == 0x15 && str[i + 3] == 0x00 && str[i + 4] == 0x60 && str[i + 6] == 0x00)
                    {
                        if (str[i + 5] == 0x01)
                        {
                            elevatorList[a].insideEleButton[height - 1].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x02)
                        {
                            elevatorList[a].insideEleButton[height - 2].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x04)
                        {
                            elevatorList[a].insideEleButton[height - 3].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x08)
                        {
                            elevatorList[a].insideEleButton[height - 4].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x10)
                        {
                            elevatorList[a].insideEleButton[height - 5].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x20)
                        {
                            elevatorList[a].insideEleButton[height - 6].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x40)
                        {
                            elevatorList[a].insideEleButton[height - 7].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x80)
                        {
                            elevatorList[a].insideEleButton[height - 8].BackColor = Color.White;
                        }

                    }
                }

                catch (IndexOutOfRangeException ea)
                {
                    Console.WriteLine(ea.Message);
                }

            }
        }
        private void openButtonMouseDown(int j)
        {
            Byte[] open = new byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x30, 0x04, 0x04, 0xDE, 0x00, 0x00 };
            Byte[] close = new byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x30, 0x04, 0x00, 0xDE, 0x00, 0x00 };
            server.Broadcast(open);
            elevatorList[j].openDoor.BackColor = Color.Red;
            //Thread.Sleep(3000);           
            //server.Broadcast(close);
            //MessageBox.Show("sent");     
        }

        private void openButtonMouseUp(int j)
        {
            elevatorList[j].openDoor.BackColor = Color.White;
            Thread.Sleep(3000);
        }

        private void closeButtonMouseDown(int j)
        {
            Byte[] close = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x30, 0x04, 0x00, 0xDE, 0x00, 0x00 };
            server.Broadcast(close);
            elevatorList[j].closeDoor.BackColor = Color.Red;
        }

        private void closeButtonMouseUp(int j)
        {
            Thread.Sleep(3000);
            elevatorList[j].closeDoor.BackColor = Color.White;
        }

        //level 1
        private void level1ButtonMouseDown(int j)
        {
            Byte[] floor1Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x01, 0x01, 0x7A, 0x00, 0x00 };
            // Byte[] floor1Byte = new Byte[23] { 0x01, 0x80, 0x15, 0x00, 0x20, 0x02, 0x02, 0x21, 0x01, 0x80, 0x15, 0x00, 0x36, 0x01, 0x01, 0x22, 0x02, 0x00, 0x15, 0x00, 0x01, 0x04, 0x04 };
            server.Broadcast(floor1Byte);
            elevatorList[j].insideEleButton[height - 1].BackColor = Color.Red;
        }

        private void level2ButtonMouseDown(int j)
        {
            Byte[] floor2Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x02, 0x02, 0x21, 0x00, 0x00 };
            server.Broadcast(floor2Byte);
            elevatorList[j].insideEleButton[height - 2].BackColor= Color.Red;
        }

        private void level3ButtonMouseDown(int j)
        {
            Byte[] floor3Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x04, 0x04, 0x6B, 0x00, 0x00 };
            server.Broadcast(floor3Byte);
            elevatorList[j].insideEleButton[height - 3].BackColor = Color.Red;
        }

        private void level4ButtonMouseDown(int j)
        {
            Byte[] floor4Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x08, 0x08, 0x89, 0x00, 0x00 };
            server.Broadcast(floor4Byte);
            elevatorList[j].insideEleButton[height - 4].BackColor = Color.Red;
        }

        private void level5ButtonMouseDown(int j)
        {
            Byte[] floor5Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x10, 0x10, 0xD3, 0x00, 0x00 };
            server.Broadcast(floor5Byte);
            elevatorList[j].insideEleButton[height - 5].BackColor = Color.Red;
        }

        private void level6ButtonMouseDown(int j)
        {
            Byte[] floor6Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x20, 0x20, 0xFB, 0x00, 0x00 };
            server.Broadcast(floor6Byte);
            elevatorList[j].insideEleButton[height - 6].BackColor = Color.Red;
        }

        private void level7ButtonMouseDown(int j)
        {
            Byte[] floor7Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x40, 0x40, 0x05, 0x00, 0x00 };
            server.Broadcast(floor7Byte);
            elevatorList[j].insideEleButton[height - 7].BackColor = Color.Red;
        }

        private void level8ButtonMouseDown(int j)
        {
            Byte[] floor8Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x80, 0x80, 0x65, 0x00, 0x00};
            server.Broadcast(floor8Byte);
            elevatorList[j].insideEleButton[height - 8].BackColor = Color.Red;
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
            byte[] sendByte = { 0x01, 0x02 };
            server.Broadcast(sendByte);

            
            addControlPanel();

          
            for (int i = 0; i < noOfElevator; i++)
            {
                int tempi = i;
                newElevator(i);
                elevatorList[i].openDoor.MouseDown += (sender1, ex) => this.openButtonMouseDown(tempi);
                elevatorList[i].openDoor.MouseUp += (sender1, ex) => this.openButtonMouseUp(tempi);
                elevatorList[i].closeDoor.MouseDown += (sender1, ex) => this.closeButtonMouseDown(tempi);
                elevatorList[i].closeDoor.MouseUp += (sender1, ex) => this.closeButtonMouseUp(tempi);

                elevatorList[i].insideEleButton[height - 1].MouseDown += (sender1, ex) => this.level1ButtonMouseDown(tempi);
                elevatorList[i].insideEleButton[height - 2].MouseDown += (sender1, ex) => this.level2ButtonMouseDown(tempi);
                elevatorList[i].insideEleButton[height - 3].MouseDown += (sender1, ex) => this.level3ButtonMouseDown(tempi);
                elevatorList[i].insideEleButton[height - 4].MouseDown += (sender1, ex) => this.level4ButtonMouseDown(tempi);
                elevatorList[i].insideEleButton[height - 5].MouseDown += (sender1, ex) => this.level5ButtonMouseDown(tempi);
                elevatorList[i].insideEleButton[height - 6].MouseDown += (sender1, ex) => this.level6ButtonMouseDown(tempi);
                elevatorList[i].insideEleButton[height - 7].MouseDown += (sender1, ex) => this.level7ButtonMouseDown(tempi);
                elevatorList[i].insideEleButton[height - 8].MouseDown += (sender1, ex) => this.level8ButtonMouseDown(tempi);
            }
          
        }
    }
}
