using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace sample_leap
{

    class Serial__communication
    {

        static SerialPort comport;

        public static string GetPortName(string portname)
        {
            string[] _portName = null;
            _portName = SerialPort.GetPortNames();
            return _portName[0];
        }
        public void sendDataSerial( string outgoingCommmad, string value)
        {
            

            comport = new SerialPort();

            comport.PortName = GetPortName(comport.PortName);
            comport.BaudRate = int.Parse("38400");
            comport.Parity = (Parity)Enum.Parse(typeof(Parity), "None", true);
            comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One", true);
            comport.Handshake = (Handshake)Enum.Parse(typeof(StopBits), "None", true);
            comport.DataBits = int.Parse("8");


            comport.DataReceived += new SerialDataReceivedEventHandler(IncomingDataHandler);
            comport.WriteTimeout = 500;
            try
            {
                comport.Open();
                Console.WriteLine("{0}", comport.PortName.ToString());
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.ToString(), "ERROR");
            }
            
          



            comport.WriteLine(String.Format( outgoingCommmad, value));

         

        }

        public static void IncomingDataHandler( object sender, SerialDataReceivedEventArgs e)
        {
           
            
            try
            {
                SerialPort serial_com = (SerialPort)sender;
                string incomingCommand = serial_com.ReadExisting();
                Console.WriteLine("incoming data from arduino: ");
                Console.Write(incomingCommand);
            }catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "ERROR");
            }
            
            
           
        }
    }
}
