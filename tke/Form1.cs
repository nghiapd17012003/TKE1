using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using SimpleTCP;
using System.IO;
using Microsoft.VisualBasic;


namespace tke
{
    public partial class Form1 : Form
    {
        int elevatorNumberInput = 0;
        int height = 8;
        int noOfElevator = 4;
        //string X = "07-40-1D-00-47";
        Boolean connectionStatus = false;
        List<Button> upButtonList = new List<Button>();
        List<Button> downButtonList = new List<Button>();
        List<Button> changeElevatorOption = new List<Button>();
        List<Elevator> elevatorList = new List<Elevator>();
        Label changeElevator = new Label();
        SimpleTcpServer server = new SimpleTcpServer();
        string currentMode = "";
        string newMode;

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

        private void option(Panel p)
        {
            for (int i = 0; i < noOfElevator; i++)
            {
                Button elevatorOption = new Button();
                elevatorOption.Location = new Point(150, 50 + 30 * i);
                elevatorOption.Size = new Size(67,30);
                elevatorOption.Text = "Elevator " + (i + 1);
                changeElevatorOption.Add(elevatorOption);
                p.Controls.Add(elevatorOption);
            }
        }



        public void addControlPanel()
        {
            Panel controlPanel = new Panel();
            controlPanel.Size = new Size(220, height * 40);
            controlPanel.Location = new Point(0, 0);
            controlPanel.BorderStyle = BorderStyle.FixedSingle;
            //changeElevator.AppendText("Change Elevator");
            //changeElevator.
            changeElevator.Text = "Change elevator";
            changeElevator.Location = new Point(150, 0);
            changeElevator.Size = new Size(65, 50);
            controlPanel.Controls.Add(changeElevator);
            upButton(controlPanel);
            downButton(controlPanel);
            levelLable(controlPanel);
            option(controlPanel);
            Controls.Add(controlPanel);
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
            //elevator.status.Text = "Stop";
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
            elevatorPanel.Controls.Add(elevator.connectingStatus);

            elevator.eventCapturer.Location = new Point(94, 200);
            elevator.eventCapturer.Size = new Size(200, 100);
            elevator.eventCapturer.Multiline = true;
            elevator.eventCapturer.ScrollBars = ScrollBars.Both;
            elevator.eventCapturer.Text = "";
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

        private void disconnectElevator()
        {
            for (int i = 0; i < height; i++)
            {
                elevatorList[elevatorNumberInput].cabinPosition[i].Visible = false;
            }

            elevatorList[elevatorNumberInput].positionScreen.Text = "";
            elevatorList[elevatorNumberInput].status.Text = "";
            elevatorList[elevatorNumberInput].connectingStatus.Text = "";
            elevatorList[elevatorNumberInput].connectingStatus.BackColor= Color.White;
            elevatorList[elevatorNumberInput].mode.Text = "";
            elevatorList[elevatorNumberInput].mode.BackColor= Color.Black;
            elevatorList[elevatorNumberInput].directionScreen.Text = "";
            
        }

        private void ClientConnected(Object sender, TcpClient e)
        {

        }

        private void Disconnected(Object sender, TcpClient e)
        {


        }

        private async void DataReceived(Object sender, SimpleTCP.Message e)
        {
            
            Byte[] str = Encoding.GetEncoding(28591).GetBytes(e.MessageString);
            var Convertdata = BitConverter.ToString(str);

            //connecting status
            connectionStatus = false;
            int z = Convertdata.Length;
            if (z > 0)
            {
                connectionStatus = true;
            }

            else { 
                connectionStatus = false;   
            }

            if(connectionStatus == true)
            {
                elevatorList[elevatorNumberInput].connectingStatus.Text = "Connected";
                elevatorList[elevatorNumberInput].connectingStatus.BackColor = Color.Green;
                connectionStatus= true;
            }      
             
            //postion
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                    {

                        elevatorList[elevatorNumberInput].positionScreen.Text = str[i + 6].ToString();
                        //elevatorList[0].cabinPosition[height - Convert.ToInt32(str[i + 6])].BackColor = Color.Blue;//height - current possition because the cabin buttons have been added from top to bottom


                        for (int j = 0; j < height; j++)
                        {
                            if (height - j == Convert.ToInt32(str[i + 6]))
                            {
                                elevatorList[elevatorNumberInput].cabinPosition[j].Visible = true;
                            }

                            else
                            {
                                elevatorList[elevatorNumberInput].cabinPosition[j].Visible = false;
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
                        elevatorList[elevatorNumberInput].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\upArrowInWhite.png");
                        elevatorList[elevatorNumberInput].status.Text = "Moving";
                    }

                    if (str[i + 5] == 0x21)
                    {
                        elevatorList[elevatorNumberInput].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\downArrowInWhite.png");
                        elevatorList[elevatorNumberInput].status.Text = "Moving";
                    }

                    if (str[i + 5] == 0x00)
                    {
                        elevatorList[elevatorNumberInput].directionScreen.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\black-color.png");
                        elevatorList[elevatorNumberInput].status.Text = "Stop";
                    }
                }
            }

            //mode
            
            DateTime currentTime = DateTime.Now;
            string timeString = currentTime.ToString(" HH:mm:ss dd/MM/yyyy            ");
            
            
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x48 && str[i + 5] == 0xFF)
                {
                    if (str[i + 6] == 0x40)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "Fire!";
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Yellow;
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.Red;
                        newMode = "Fire";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText(currentMode + timeString);
                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                        }
                    }
                    else if (str[i + 6] == 0x00)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "Auto";
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.White;
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Black;
                        newMode = "Auto";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText( currentMode + timeString);
                            //StreamWriter writer = new StreamWriter(@"C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt");
                            //writer.WriteLine(currentMode + timeString);
                            //writer.WriteLine("hallo");
                            //writer.Close();

                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                        }
                    }

                    else if (str[i + 6] == 0x20)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "Priority";
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Yellow;
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.Red;
                        newMode = "Priority";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText("\n" + currentMode + timeString);
                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                        }
                    }

                    else if (str[i + 6] == 0x80)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "Full Load!";
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Yellow;
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.Red;
                        newMode = "Full Load";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText(currentMode + timeString);
                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                            
                        }
                    }
                    else if (str[i + 6] == 0x90)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "Over Load!";
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Yellow;
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.Red;
                        newMode = "Over Load";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText(currentMode + timeString);
                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                        }
                    }

                    else if (str[i + 6] == 0x08)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "Out Of Order!";
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Yellow;
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.Red;
                        newMode = "Out of Order";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText(currentMode + timeString);
                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                        }
                    }
                }

                else if (str[i] == 0x07 && str[i + 1] == 0x40 && str[i + 2] == 0x1D && str[i + 3] == 0x00 && str[i + 4] == 0x47)
                {
                    if (str[i + 8] == 0x94)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "JU";
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Black;
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.Red;
                        newMode = "JU";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText(currentMode + timeString);
                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                        }
                    }
                    else if (str[i + 8] == 0x92)
                    {
                        elevatorList[elevatorNumberInput].mode.Text = "IF";
                        elevatorList[elevatorNumberInput].mode.BackColor = Color.Yellow;
                        elevatorList[elevatorNumberInput].mode.ForeColor = Color.Red;
                        newMode = "IF";
                        if (String.Equals(currentMode, newMode) == false)
                        {
                            currentMode = newMode;
                            elevatorList[elevatorNumberInput].eventCapturer.AppendText(currentMode + timeString);
                            File.AppendAllText("C:\\Users\\nghia\\OneDrive\\Desktop\\project\\TKE1\\statusRecord.txt", "\n" + currentMode + timeString);
                        }

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
                            elevatorList[elevatorNumberInput].elevatorDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\closeDoor.png");
                            for (int j = 0; j < height; j++)
                            {
                                elevatorList[elevatorNumberInput].cabinPosition[j].Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\closeDoorMini.png");
                            }
                        }

                        else if (str[i + 6] == 0x04)
                        {
                            elevatorList[elevatorNumberInput].elevatorDoor.Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\openDoor.png");
                            for (int j = 0; j < height; j++)
                            {
                                elevatorList[elevatorNumberInput].cabinPosition[j].Image = Image.FromFile(@"C:\Users\nghia\OneDrive\Desktop\project\TKE1\tke\image\openDoorMini.png");
                            }
                        }
                    }
                }

                catch (IndexOutOfRangeException ea)
                {
                    Console.WriteLine(ea.Message);
                }


            }

            //inside button light
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    if (str[i] == 0x01 && str[i + 1] == 0x00 && str[i + 2] == 0x15 && str[i + 3] == 0x00 && str[i + 4] == 0x60 && str[i + 6] == 0x00)
                    {
                        if (str[i + 5] == 0x01)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 1].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x02)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 2].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x04)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 3].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x08)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 4].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x10)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 5].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x20)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 6].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x40)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 7].BackColor = Color.White;
                        }

                        else if (str[i + 5] == 0x80)
                        {
                            elevatorList[elevatorNumberInput].insideEleButton[height - 8].BackColor = Color.White;
                        }

                    }
                }

                catch (IndexOutOfRangeException ea)
                {
                    Console.WriteLine(ea.Message);
                }

            }

            //light external call up
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    if (str[i] == 0x02 && str[i + 1] == 0x00 && str[i + 2] == 0x15 && str[i + 4] == 0x01 && str[i + 5] == 0x01 && str[i + 6] == 0x00)
                    {
                        switch (str[i + 3])
                        {
                            case 0x01:
                                upButtonList[height - 1].BackColor = Color.White;
                                break;

                            case 0x02:
                                upButtonList[height - 2].BackColor = Color.White;
                                break;

                            case 0x03:
                                upButtonList[height - 3].BackColor = Color.White;
                                break;

                            case 0x04:
                                upButtonList[height - 4].BackColor = Color.White;
                                break;

                            case 0x05:
                                upButtonList[height - 5].BackColor = Color.White;
                                break;

                            case 0x06:
                                upButtonList[height - 6].BackColor = Color.White;
                                break;

                            case 0x07:
                                upButtonList[height - 7].BackColor = Color.White;
                                break;
                        }
                    }
                }

                catch (IndexOutOfRangeException ea)
                {
                    Console.WriteLine(ea.Message);
                }
            }

            //light external call down
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    if (str[i] == 0x02 && str[i + 1] == 0x00 && str[i + 2] == 0x15 && str[i + 4] == 0x01 && str[i + 5] == 0x02 && str[i + 6] == 0x00)
                    {
                        switch (str[i + 3])
                        {
                            case 0x02:
                                downButtonList[height - 2].BackColor = Color.White;
                                break;

                            case 0x03:
                                downButtonList[height - 3].BackColor = Color.White;
                                break;

                            case 0x04:
                                downButtonList[height - 4].BackColor = Color.White;
                                break;

                            case 0x05:
                                downButtonList[height - 5].BackColor = Color.White;
                                break;

                            case 0x06:
                                downButtonList[height - 6].BackColor = Color.White;
                                break;

                            case 0x07:
                                downButtonList[height - 7].BackColor = Color.White;
                                break;

                            case 0x08:
                                downButtonList[height - 8].BackColor = Color.White;
                                break;
                        }
                    }
                }

                catch (IndexOutOfRangeException ea)
                {
                    Console.WriteLine(ea.Message);
                }
            }

            //eventCapture in real time

        }

        private void changeElevatorFunction()
        {
            MessageBox.Show("ok");
        }

        private void openButtonMouseDown(int j)
        {
            Byte[] open = new byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x30, 0x04, 0x04, 0xDE, 0x00, 0x00 };
            Byte[] close = new byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x30, 0x04, 0x00, 0xDE, 0x00, 0x00 };
            server.Broadcast(open);
            server.Broadcast(close);    
            elevatorList[j].openDoor.BackColor = Color.Red;
            //Thread.Sleep(3000);           
            //server.Broadcast(close);
            //MessageBox.Show("sent");     
        }

        private void chooseElevator1()
        {
            disconnectElevator();
            elevatorNumberInput = 0;
        }

        private void chooseElevator2()
        {
            disconnectElevator();
            elevatorNumberInput = 1;
        }

        private void chooseElevator3()
        {
            disconnectElevator();
            elevatorNumberInput = 2;
        }

        private void chooseElevator4()
        {
            disconnectElevator();
            elevatorNumberInput = 3;
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
            elevatorList[j].insideEleButton[height - 2].BackColor = Color.Red;
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
            Byte[] floor8Byte = new Byte[13] { 0x08, 0x00, 0x00, 0x01, 0x80, 0x15, 0x00, 0x20, 0x80, 0x80, 0x65, 0x00, 0x00 };
            server.Broadcast(floor8Byte);
            elevatorList[j].insideEleButton[height - 8].BackColor = Color.Red;
        }

        public void externalCallUp1()
        {
            Byte[] up1 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x81, 0x15, 0x01, 0x00, 0x01, 0x01, 0x03, 0x00, 0x00 };
            Byte[] up1part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x81, 0x15, 0x01, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(up1);
            server.Broadcast(up1part2);
            upButtonList[height - 1].BackColor = Color.Red;
        }

        public void externalCallUp2()
        {
            Byte[] up2 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x82, 0x15, 0x02, 0x00, 0x01, 0x01, 0x03, 0x00, 0x00 };
            Byte[] up2part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x82, 0x15, 0x02, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(up2);
            server.Broadcast(up2part2);
            upButtonList[height - 2].BackColor = Color.Red;
        }

        public void externalCallUp3()
        {
            Byte[] up3 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x83, 0x15, 0x03, 0x00, 0x01, 0x01, 0x03, 0x00, 0x00 };
            Byte[] up3part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x83, 0x15, 0x03, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(up3);
            server.Broadcast(up3part2);
            upButtonList[height - 3].BackColor = Color.Red;  
        }

        public void externalCallUp4()
        {
            Byte[] up4 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x84, 0x15, 0x04, 0x00, 0x01, 0x01, 0x03, 0x00, 0x00 };
            Byte[] up4part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x84, 0x15, 0x04, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(up4);
            server.Broadcast(up4part2);
            upButtonList[height - 4].BackColor = Color.Red;
        }

        public void externalCallUp5()
        {
            Byte[] up5 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x85, 0x15, 0x05, 0x00, 0x01, 0x01, 0x03, 0x00, 0x00 };
            Byte[] up5part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x85, 0x15, 0x05, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(up5);
            server.Broadcast(up5part2);
            upButtonList[height - 5].BackColor = Color.Red;
        }

        public void externalCallUp6()
        {
            Byte[] up6 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x86, 0x15, 0x06, 0x00, 0x01, 0x01, 0x03, 0x00, 0x00 };
            Byte[] up6part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x86, 0x15, 0x06, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(up6);
            server.Broadcast(up6part2);
            upButtonList[height - 6].BackColor = Color.Red;
        }

        public void externalCallUp7()
        {
            Byte[] up7 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x87, 0x15, 0x07, 0x00, 0x01, 0x01, 0x03, 0x00, 0x00 };
            Byte[] up7part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x87, 0x15, 0x07, 0x00, 0x01, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(up7);
            server.Broadcast(up7part2);
            upButtonList[height - 7].BackColor = Color.Red;
        }


        public void externalCallDown2() 
        {
            Byte[] down2 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x82, 0x15, 0x02, 0x00, 0x02, 0x02, 0x03, 0x00, 0x00 };
            Byte[] down2part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x82, 0x15, 0x02, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(down2);
            
            server.Broadcast(down2part2);
            downButtonList[height - 2].BackColor = Color.Red;
        }

        public void externalCallDown3()
        {
            Byte[] down3 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x83, 0x15, 0x03, 0x00, 0x02, 0x02, 0x03, 0x00, 0x00 };
            Byte[] down3part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x83, 0x15, 0x03, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(down3);
            server.Broadcast(down3part2);
            downButtonList[height - 3].BackColor = Color.Red;
        }

        public void externalCallDown4()
        {
            Byte[] down4 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x84, 0x15, 0x04, 0x00, 0x02, 0x02, 0x03, 0x00, 0x00 };
            Byte[] down4part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x84, 0x15, 0x04, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(down4);
            server.Broadcast(down4part2);
            downButtonList[height - 4].BackColor = Color.Red;
        }

        public void externalCallDown5()
        {
            Byte[] down5 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x85, 0x15, 0x05, 0x00, 0x02, 0x02, 0x03, 0x00, 0x00 };
            Byte[] down5part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x85, 0x15, 0x05, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(down5);
            server.Broadcast(down5part2);
            downButtonList[height - 5].BackColor = Color.Red;
        }

        public void externalCallDown6()
        {
            Byte[] down6 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x86, 0x15, 0x06, 0x00, 0x02, 0x02, 0x03, 0x00, 0x00 };
            Byte[] down6part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x86, 0x15, 0x06, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(down6);
            server.Broadcast(down6part2);
            downButtonList[height - 6].BackColor = Color.Red;
        }

        public void externalCallDown7()
        {
            Byte[] down7 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x87, 0x15, 0x07, 0x00, 0x02, 0x02, 0x03, 0x00, 0x00 };
            Byte[] down7part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x87, 0x15, 0x07, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(down7);
            server.Broadcast(down7part2);
            downButtonList[height - 7].BackColor = Color.Red;
        }

        public void externalCallDown8()
        {
            Byte[] down8 = new Byte[13] { 0x08, 0x00, 0x00, 0x02, 0x88, 0x15, 0x08, 0x00, 0x02, 0x02, 0x03, 0x00, 0x00 };
            Byte[] down8part2 = new byte[13] { 0x08, 0x00, 0x00, 0x02, 0x88, 0x15, 0x08, 0x00, 0x02, 0x00, 0x03, 0x00, 0x00 };
            server.Broadcast(down8);
            server.Broadcast(down8part2);
            downButtonList[height - 8].BackColor = Color.Red;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IPAddress ipHost = IPAddress.Parse("192.168.0.2");
            int tempport = 10011;


            server = new SimpleTcpServer();
            server.StringEncoder = Encoding.GetEncoding(28591);//Để nhận được các dữ liệu byte lớn hơn 7F
            //server.Start(ipHost, tempport);
            
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
            changeElevator.MouseDown += (sender1, ex) => this.changeElevatorFunction();

            upButtonList[height - 1].MouseDown += (sender1, ex) => this.externalCallUp1();
            upButtonList[height - 2].MouseDown += (sender1, ex) => this.externalCallUp2();
            upButtonList[height - 3].MouseDown += (sender1, ex) => this.externalCallUp3();
            upButtonList[height - 4].MouseDown += (sender1, ex) => this.externalCallUp4();
            upButtonList[height - 5].MouseDown += (sender1, ex) => this.externalCallUp5();
            upButtonList[height - 6].MouseDown += (sender1, ex) => this.externalCallUp6();
            upButtonList[height - 7].MouseDown += (sender1, ex) => this.externalCallUp7();

            downButtonList[height - 2].MouseDown += (sender1, ex) => this.externalCallDown2();
            downButtonList[height - 3].MouseDown += (sender1, ex) => this.externalCallDown3();
            downButtonList[height - 4].MouseDown += (sender1, ex) => this.externalCallDown4();
            downButtonList[height - 5].MouseDown += (sender1, ex) => this.externalCallDown5();
            downButtonList[height - 6].MouseDown += (sender1, ex) => this.externalCallDown6();
            downButtonList[height - 7].MouseDown += (sender1, ex) => this.externalCallDown7();
            downButtonList[height - 8].MouseDown += (sender1, ex) => this.externalCallDown8();

            changeElevatorOption[0].MouseDown += (sender1, ex) => this.chooseElevator1();
            changeElevatorOption[1].MouseDown += (sender1, ex) => this.chooseElevator2();
            changeElevatorOption[2].MouseDown += (sender1, ex) => this.chooseElevator3();
            changeElevatorOption[3].MouseDown += (sender1, ex) => this.chooseElevator4();
        }
        
    } 
}
