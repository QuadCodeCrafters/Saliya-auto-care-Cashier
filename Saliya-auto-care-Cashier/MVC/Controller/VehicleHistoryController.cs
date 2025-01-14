using System;
using System.Collections.Generic;
using Saliya_auto_care_Cashier.MVC.Model;

namespace Saliya_auto_care_Cashier.MVC.Controller
{
    public class VehicleHistoryController
    {
        private VehicleHistoryModel model;

        public VehicleHistoryController()
        {
            model = new VehicleHistoryModel();
        }

        public VehicleHistoryModel.VehicleInfo GetVehicleInfo(string vehicleNumber)
        {
            return model.GetVehicleInfo(vehicleNumber);
        }

        public List<VehicleHistoryModel.ServiceHistory> GetServiceHistory(string vehicleNumber, string date = null)
        {
            return model.GetServiceHistory(vehicleNumber, date);
        }

        public List<string> GetServiceDates(string vehicleNumber)
        {
            return model.GetServiceDates(vehicleNumber);
        }

        public Dictionary<string, decimal> GetTotals(string vehicleNumber, string date = null)
        {
            return model.GetTotals(vehicleNumber, date);
        }
    }
}

