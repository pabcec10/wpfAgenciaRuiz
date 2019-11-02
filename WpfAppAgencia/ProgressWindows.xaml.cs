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
using System.Windows.Threading;


namespace WpfAppAgencia
{
    /// <summary>
    /// Lógica de interacción para ProgressWindows.xaml
    /// </summary>
    public partial class ProgressWindows : Window
    {
        public ProgressWindows()
        {
            InitializeComponent();
        }
    }
    public class ProgressManager
    {
        private Thread thread;
        private bool canAbortThread = false;
        private ProgressWindows window;

        public void BeginWaiting()
        {
            this.thread = new Thread(this.RunThread);
            this.thread.IsBackground = true;
            this.thread.SetApartmentState(ApartmentState.STA);
            this.thread.Start();
        }
        public void EndWaiting()
        {
            if (this.window!=null)
            {
                this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => 
                { this.window.Close(); }));
                while (!this.canAbortThread) { };
            }
            this.thread.Abort();
        }
        public void RunThread()
        {
            this.window = new ProgressWindows();
            this.window.Closed += new EventHandler(waitingWindows_Closed);
            this.window.ShowDialog();

        }
        public void ChangeStatus(string text)
        {
            if (this.window!=null)
            {
                this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => 
                { this.window.StatusText.Text = text; }));
            }
        }
        public void ChangeProgress(double value)
        {
            if (this.window!=null)
            {
                this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => 
                { this.window.Progress.Value=value; }));
            }
        }
        public void SetProgressMaxValue(double MaxValue)
        {
            Thread.Sleep(100);
            if (this.window!=null)
            {
                this.window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
                 {
                     this.window.Progress.Minimum = 0;
                     this.window.Progress.Maximum = MaxValue;
                 }));
            }
        }
        void waitingWindows_Closed(object sender, EventArgs e)
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();
            this.canAbortThread = true;
        }

    }
}
