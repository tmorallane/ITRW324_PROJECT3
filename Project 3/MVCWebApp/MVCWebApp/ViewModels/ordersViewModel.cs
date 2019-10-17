using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebApp.ViewModels
{
    public class ordersViewModel
    {
        public int total_orders { get; set; }

        public double total_sales { get; set;}

        public int Africa_market { get; set; }

        public int Asia_market { get; set; }

        public int Europe_market { get; set; }

        public int America_market { get; set; }
    }
}