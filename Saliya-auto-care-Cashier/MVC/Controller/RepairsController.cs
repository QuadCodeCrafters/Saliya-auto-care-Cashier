using Saliya_auto_care_Cashier.MVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saliya_auto_care_Cashier.MVC.Controller
{
    internal class RepairsController
    {
        private readonly RepairsModel repairsModel;

        public RepairsController()
        {
            repairsModel = new RepairsModel();
        }

        public List<(string Name, decimal Price)> GetCategories()
        {
            return repairsModel.GetCategories();
        }
    }
}
