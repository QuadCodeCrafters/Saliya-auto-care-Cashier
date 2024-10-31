using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saliya_auto_care_Cashier.MVVM.Model;



namespace Saliya_auto_care_Cashier.MVVM.ViewModel
{
    public class Viewmodel
    {

        public static PassClass getTotalPrice(PassClass obj)
        {

            obj.setPassword(obj.getPassword());
            return obj;

        }


    }
}
