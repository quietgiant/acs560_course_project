using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace EZ_Inventory
{
    class BarcodeReader
    {
       public SerialPort BarcodeReaderPortConnection;
        Thread thread2;
        public BarcodeReader(string comPort)
        {
            if (comPort != null && comPort != "")
            {
                BarcodeReaderPortConnection = new SerialPort();
                var BaudRate = 9800;
                BarcodeReaderPortConnection.PortName = comPort;
                BarcodeReaderPortConnection.BaudRate = BaudRate;
            }
         
        }
        public void openConnection()
        {
            BarcodeReaderPortConnection.Open();
        }
        public void CloseConnection()
        {
            BarcodeReaderPortConnection.Dispose(); 
        }
        public void killBarcodeReaderThread()
        {
            thread2.Abort();
        }
     
        public void activateBarcodeReadToTextBox(TextBox ComPortInput, Action<string> callback)
        {
            try
            {
                BarcodeReaderPortConnection.Open();
                BarcodeReaderPortConnection.DiscardInBuffer();
                 thread2 = new Thread(() =>
                {
                    Thread.Sleep(100);
                    while (true)
                    {
                        try
                        {
                            string UPC = BarcodeReaderPortConnection.ReadLine();

                           UPC = UPC.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                            callback(UPC);
                        }
                        catch(Exception ex)
                        {

                        }
                      
                    }

                });
                thread2.IsBackground = true;
                thread2.Start();
            }
            catch(Exception e)
            {
                if (e.Message.Substring(0,8) == "The port") {
                    string message = "EZinventory does not recognize the specified Com Port ("+ ComPortInput.Text + "). Please re-enter the comport in the settings menu. We are disabling the ComPort so you can continue to use our application.";
                    string title = "No Barcode Scanner";
                    MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                    ComPortInput.Text = "";
                }
                else {
                    MessageBoxResult result = MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }


        }

   
    
    }
}
