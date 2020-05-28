using System;
using System.Management;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Text;

namespace GVS
{
    public partial class MainForm : Form
    {


        public List<string> inList = new List<string>();
        public int count;
        public MainForm()
        {
            InitializeComponent();
            chart1.Series[0].LegendText = "Degrees";
            chart1.Series[0].ChartType = SeriesChartType.Line;
           String FilePath = "C:/Temp/ACSV.csv";
            Console.Read();
            ManagementObjectSearcher mgmtObjSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection colDisks = mgmtObjSearcher.Get();


            //MAY BE ABLE TO ACTUALLY LOOK THROUGH COMPORTS, too tired going to bed. Last night you couldn't 
            //look at ports because the the loop fucked it dunno why. this doesn't. Investigate.
            // ALSO TODO: Make the stop button work.
            foreach (ManagementObject x in colDisks)
            {
                Console.WriteLine("k");
            }

        }



        private void startComPort(string comPort)
        {

            SerialPort mySerialPort = new SerialPort("COM"+comPort);
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            mySerialPort.Open();

        }


        private void DataReceivedHandler(
         object sender,
         SerialDataReceivedEventArgs e
         )
        {
            SerialPort sp = (SerialPort)sender;
           
            string indata = sp.ReadLine();
            //indata = indata + ",";
            var src = DateTime.Now;
            var hm = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, src.Second, src.Millisecond, 0);
            var shm = hm.ToString();
            File.AppendAllText("C:/Temp/ACSV.csv",src.Hour+":"+src.Minute+":"+src.Second+":"+src.Millisecond+"," + indata+",");

            //this.TextBox1.Text = indata;

            inList.Add(indata);
            SetText(indata);
            System.Threading.Thread.Sleep(8);
            inList.Clear();
            
        }


        public delegate void SetTextCallback(string text);
        public SetTextCallback _SetTextCallback;


        private void SetText(String text)
        {

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.chart1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                decimal test = 0;
                if (decimal.TryParse(text,out test)){
                    if (decimal.Parse(text) < 1 && decimal.Parse(text) > -1)
                    {
                        //decimal y = decimal.Parse(text);
                        chart1.Series[0].Points.AddY(decimal.Parse(inList[inList.Count - 1]));
                        textBox1.Text = inList[inList.Count - 1];
                        count++;
                        if(count > 2000)
                        {
                            chart1.Series[0].Points.Clear();
                            count = 0;
                        }
                    }
                }
                
                
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            int outParam;
            if (int.TryParse(ComEntry.Text, out outParam)){
                startComPort(ComEntry.Text);
            }
            else {
                MessageBox.Show("Enter Numeric value for comport./n Check device manager for possible" +
                "port number. ");
                 }
        }
    }
}
