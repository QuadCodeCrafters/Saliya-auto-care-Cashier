using Microsoft.AspNet.SignalR.Client;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class DelivaryService_View : UserControl
    {
        private IHubProxy _hubProxy;
        private HubConnection _connection;

        public DelivaryService_View()
        {
            InitializeComponent();
           // InitializeSignalR();
        }

        private void InitializeSignalR()
        {
            _connection = new HubConnection("https://SaliyaSignalRService.azurewebsites.net");
            _hubProxy = _connection.CreateHubProxy("MyHub");

            _hubProxy.On<string>("UpdateUI", (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    // Update your UI here
                    MessageBox.Show("Button Pressed on Web App: " + message);
                });
            });

            _connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("There was an error opening the connection: " + task.Exception.GetBaseException());
                    });
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Connected to SignalR");
                    });
                }
            });
        }


    }
}
