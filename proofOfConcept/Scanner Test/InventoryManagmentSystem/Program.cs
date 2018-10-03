using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryManagmentSystem
{
    class Program
    {
        public static void Main()
        {
            bool Continue = true;
            SerialPort mySerialPort = new SerialPort();

            mySerialPort.PortName = "COM3";
            mySerialPort.BaudRate = 9800;

            mySerialPort.Open();

   
       
            while (Continue)
            {
                
                using (StreamWriter writer = new StreamWriter("TestLog.txt", true))
                {

                    string msg = mySerialPort.ReadLine();
                    writer.Write("PLU: " + msg);

                }
            }

        }
    }
}
