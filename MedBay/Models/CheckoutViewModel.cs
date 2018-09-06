using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedBay.Models
{
    public class CheckoutViewModel
    {
        public List<Cart> CartItems { get; set; }
        public int TotalCartPrice { get; set; }
    }
}