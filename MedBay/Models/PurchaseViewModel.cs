using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedBay.DAL.Entity;

namespace MedBay.Models
{
    public class PurchaseViewModel
    {
        public Cart CartItem { get; set; }
        public Order OrderItem { get; set; }
        public int TotalPrice { get; set; }
    }
}