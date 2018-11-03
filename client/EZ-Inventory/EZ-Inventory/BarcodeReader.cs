using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Controls;

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
        public void activateBarcodeReadToTextBox(Action<string> callback)
        {

            BarcodeReaderPortConnection.Open();
            BarcodeReaderPortConnection.DiscardInBuffer();
            Thread thread2 = new Thread(() =>
            {
              
                Thread.Sleep(100);
      
                while (true)
                {
                   
                    string UPC = BarcodeReaderPortConnection.ReadLine();
                    callback(UPC);
                }


            });
            thread2.IsBackground = true;
            thread2.Start();
      


        }

   
    
    }
}
