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
using System.Windows.Shapes;
using System.Threading;

namespace WpfAppAgencia
{
    /// <summary>
    /// Lógica de interacción para frmMenuPpal.xaml
    /// </summary>
    public partial class frmMenuPpal : Window
    {
        public frmMenuPpal()
        {
            InitializeComponent();
            button.Visibility = Visibility.Hidden;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Close();
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            frmParamEstCivil f = new frmParamEstCivil();
            f.Owner = this;
            f.ShowDialog();
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            frmParamFeriados f = new frmParamFeriados();
            f.Owner = this;
            f.ShowDialog();
        }
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            frmParamMarcas f = new frmParamMarcas();
            f.Owner = this;
            f.ShowDialog();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            ProgressManager pm = new ProgressManager();
            pm.BeginWaiting();
            pm.SetProgressMaxValue(10);
            for (int i=0;i<10;i++)
            {
                pm.ChangeStatus("Loading " + i.ToString() + " from 10");
                pm.ChangeProgress(i);
                Thread.Sleep(1000);
            }
            pm.EndWaiting();
        }
    }
}
