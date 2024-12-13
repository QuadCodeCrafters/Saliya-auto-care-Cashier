using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Saliya_auto_care_Cashier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dashboard DashboardInstance { get; internal set; }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker bk = new BackgroundWorker();
            bk.WorkerReportsProgress = true;
            bk.DoWork += bk_DoWork;
            bk.ProgressChanged += bk_ProgressChanged;
            bk.RunWorkerAsync();
        }
        void bk_DoWork(object sender, EventArgs e)
        {
            //A Thread for Run the Loading
            for (int i = 0; i <= 50; i++)
            {

                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(50);
            }
        }

        void bk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            ProgressBar.Value = e.ProgressPercentage;
            if (ProgressBar.Value == 50)
            {
                Loginpage lP = new Loginpage();
                lP.Show();
                this.Close();
            }

        }
    }
}
