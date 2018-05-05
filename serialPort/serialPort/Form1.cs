using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serialPort
{
    
    public partial class Truck_scales : Form
    {
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while(true)
            {
                Thread.Sleep(500);
                backgroundWorker1.ReportProgress(1);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            textBox1.Text += "A" + Environment.NewLine;
        }

        void startWorker()
        {
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
        }

        #region SeriaPort_1
        void openSeriaPort_1()
        {
            SerialPort mySerialPort = new SerialPort("COM1");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            mySerialPort.Close();
        }
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Debug.Print("Data Received:");
            Debug.Print(indata);
        }
        #endregion

        #region SeriaPort_2
        string InputData = String.Empty;
        System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort();
        delegate void SetTextCallback(string text);

        void openSeriaPort_2()
        {
            Set_SeriaPort_2();
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived_1);

        }
        // private void ReadSample3_Load(object sender, EventArgs e)
        private void Set_SeriaPort_2()
        {
            if (port.IsOpen)
                port.PortName = "COM1";
            //stsStatus.Text = port.PortName + ": 9600,8N1";
 
            // try to open the selected port:
            try
            {
               port.PortName = "COM1";
 
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DtrEnable = true;
                port.Handshake = Handshake.None;
                port.DtrEnable = true;
                port.Open();
                //If i set port.Handshake= Handshake.RequestToSend and requestToSend= True so Data IS Not Showing In The Hyper Terminal MAY BE DATA IS NOT RECEIVING/////////
                //port.BaudRate = 9600;
                //port.DataBits=
            }
            // give a message, if the port is not available:
            catch
            {
                MessageBox.Show("Serial port " + port.PortName +
                "cannot be opened!", "RS232 tester",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //comboBox1.SelectedText = "";
                // stsStatus.Text = "Select serial port!";
            }
        }
        private void port_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            InputData = port.ReadExisting();
            //byte[] receiveBuffer = new byte[128];
            //int bufferIndex = 0;
            //int bytesRead = 0;
            //int startPacketIndex = 0;
            //int expectedPacketLength = -1;
            //bool expectedPacketLengthIsSet = false;
            //int numBytesToRead = receiveBuffer.Length;
            //bytesRead += port.Read(receiveBuffer, bufferIndex, numBytesToRead);
            if (InputData != String.Empty)
            {
                // txtIn.Text = InputData;
                // because of different threads this
                // does not work properly !!
                SetText(InputData);
            }
        }
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else this.textBox1.Text += text;
        }

        #endregion

        #region SeriaPort_3
        void openSeriaPort_3()
        {
            Set_SeriaPort_3();
        }
        private void Set_SeriaPort_3()
        {
            //  Check if the data string contains the data that you want
            //bool hasData = dataString.Contains("connectAlready");
            string[] ports = SerialPort.GetPortNames();
            SerialPort[] serialport = new SerialPort[ports.Length];
            foreach(string p in ports)
            {
                int i = Array.IndexOf(ports, p);
                serialport[i] = new SerialPort(); //note this line, otherwise you have no serial port declared, only array reference which can contains real SerialPort object
                serialport[i].PortName = p; 
                serialport[i].BaudRate = 9600;
                serialport[i].Open();
                //Scan inputs for "connectAlready"
                serialport[i].DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived); //This is to add event handler delegate when data is received by the port
            }
        }
        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort1 = sender as SerialPort;
            byte[] data = new byte[serialPort1.BytesToRead]; //this is to provide the data buffer
            Stream portStream = serialPort1.BaseStream;
            portStream.Read(data, 0, data.Length); //You get your data from serial port as byte[]
                                                   //Do something on your data
        }
        //Encoding.UTF8.GetString to convert the input data from byte[] to ASCII characters(Edit: 
        //    consider of changing/skip this step and step 2 if the data received is not byte[] but ASCII)
        //private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    SerialPort serialPort1 = sender as SerialPort;
        //    byte[] data = new byte[serialPort1.BytesToRead]; //this is to provide the data buffer
        //    Stream portStream = serialPort1.BaseStream;
        //    portStream.Read(data, 0, data.Length); //You get your data from serial port as byte[]
        //    string dataString = Encoding.UTF8.GetString(data); //here is your data in string
        //                                                       //Do something on your data
        //}
        #endregion
        public Truck_scales()
        {
            InitializeComponent();
            startWorker();
        }

        private void Truck_scales_Load(object sender, EventArgs e)
        {
            //Output: 1,234.00
            //txtTestPort1.Text = string.Format("{0:n}", 2435);
            
            //Output: 9,876
            //txtTestPort2.Text = string.Format("{0:n0}",2435);
            
            //Output: 9,876
            txtTestPort1.Text = 2435.ToString("#,#");
            
            //Output: 9,876.00
            txtTestPort2.Text = 2435.ToString("#,#.00");
        }
    }
}
