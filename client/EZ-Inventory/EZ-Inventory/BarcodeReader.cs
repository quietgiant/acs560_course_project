using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZ_Inventory
{
    class BarcodeReader
    {
       public SerialPort BarcodeReaderPortConnection;
        public BarcodeReader(string comPort)
        {
            BarcodeReaderPortConnection = new SerialPort();
            var BaudRate = 9800;
            BarcodeReaderPortConnection.PortName = comPort;
            BarcodeReaderPortConnection.BaudRate = BaudRate;
        }
        public void openConnection()
        {
            BarcodeReaderPortConnection.Open();
        }
        public string readFromConnection()
        {
           return BarcodeReaderPortConnection.ReadLine();
        }
    }
}
