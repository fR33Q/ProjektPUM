using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedBay.DAL.Entity;

namespace MedBay.Models
{
    public class SummaryViewModel
    {
        public Order OrderItem { get; set; }
        public List<Cart> CartItems { get; set; }
        
    }
}