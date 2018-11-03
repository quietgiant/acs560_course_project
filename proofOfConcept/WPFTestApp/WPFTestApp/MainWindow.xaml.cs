using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace WPFTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        public void A()
        {
            Thread.Sleep(100);
            int i = 0;
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    FirstTabText.Text += "\na:" + i;
                   
                });
                
                i++;
            }
        }
        public void B()
        {
            Thread.Sleep(1000);
            int i = 0;
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    SecondTabText.Text += "\nb:" + i;

                });

                i++;
            }
            
        }
        private void Btn_FirstTab_Click(object sender, RoutedEventArgs e)
        {
            tab1.IsSelected = true;
            FirstTabText.Text = "test";
            Thread thread2 = new Thread(new ThreadStart(A));
            thread2.Start();

        }
        private void Btn_SecondTab_Click(object sender, RoutedEventArgs e)
        {
            tab2.IsSelected = true;
            Thread thread3 = new Thread(new ThreadStart(B));
            thread3.Start();
        }
    }
}
