using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    public class DatabaseStringModel
    {
        public string ConnectionString { get; private set; }

        public DatabaseStringModel()
        {
            // Define the connection string 
          ConnectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";
        }
    }
}
