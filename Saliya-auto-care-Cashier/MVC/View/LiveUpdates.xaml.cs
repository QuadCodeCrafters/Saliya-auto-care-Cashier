using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using Saliya_auto_care_Cashier.MVC.Model;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class LiveUpdates : UserControl
    {

        private readonly DatabaseStringModel conn;  // DatabaseStringModel
        private List<string> offers;
        private int currentOfferIndex = 0;
        private DispatcherTimer updateTimer;
        private DispatcherTimer changeOfferTimer;

        public LiveUpdates()
        {
            InitializeComponent();
            conn = new DatabaseStringModel(); // conn
            offers = new List<string>();
            LoadOffers();

            // Timer for updating offers from database
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMinutes(1);
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

            // Timer for changing displayed offer
            changeOfferTimer = new DispatcherTimer();
            changeOfferTimer.Interval = TimeSpan.FromSeconds(15);
            changeOfferTimer.Tick += ChangeOfferTimer_Tick;
            changeOfferTimer.Start();

            DisplayCurrentOffer();
        }

        private void LoadOffers()
        {
            string connectionString = conn.ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT offer_text FROM offers_table";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        offers.Clear(); // Clear existing offers before reloading
                        while (reader.Read())
                        {
                            offers.Add(reader.GetString("offer_text"));
                        }
                    }
                }
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            LoadOffers(); // Reload offers from the database every minute
        }

        private void ChangeOfferTimer_Tick(object sender, EventArgs e)
        {
            if (offers.Count > 0)
            {
                currentOfferIndex = (currentOfferIndex + 1) % offers.Count;
                DisplayCurrentOffer();
            }
        }

        private void DisplayCurrentOffer()
        {
            if (offers.Count > 0)
            {
                offertext.Text = offers[currentOfferIndex];
            }
            else
            {
                offertext.Text = "No offers available";
            }
        }

        private void PreviousOffer_Click(object sender, RoutedEventArgs e)
        {
            if (offers.Count > 0)
            {
                currentOfferIndex = (currentOfferIndex - 1 + offers.Count) % offers.Count;
                DisplayCurrentOffer();
                ResetChangeOfferTimer();
            }
        }

        private void NextOffer_Click(object sender, RoutedEventArgs e)
        {
            if (offers.Count > 0)
            {
                currentOfferIndex = (currentOfferIndex + 1) % offers.Count;
                DisplayCurrentOffer();
                ResetChangeOfferTimer();
            }
        }

        private void ResetChangeOfferTimer()
        {
            // Reset the timer when manually changing offers
            changeOfferTimer.Stop();
            changeOfferTimer.Start();
        }
    }
}

