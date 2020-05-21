using System;
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

namespace GVS
{
    public partial class MainForm : Form
    {


        public List<string> inList = new List<string>();

        public MainForm()
        {
            InitializeComponent();

            SerialPort mySerialPort = new SerialPort("COM5");
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            mySerialPort.Open();
            chart1.Series[0].LegendText = "Degrees";
            chart1.Series[0].ChartType = SeriesChartType.Line;
            //chart1.Series[0].IsValueShownAsLabel = true;

            
           String FilePath = "C:/Temp/ACSV.csv";
           //FileStream Fs = File.Create("C:/Temp/ACSV.csv");
            Console.Read();


            //mySerialPort.Close();

        }

        private void DataReceivedHandler(
         object sender,
         SerialDataReceivedEventArgs e
         )
        {
            SerialPort sp = (SerialPort)sender;
           
            string indata = sp.ReadLine();
            //indata = indata + ",";
      
            File.AppendAllText("C:/Temp/ACSV.csv", indata);

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
                    }
                }
                
                
            }
        }


    }
}
