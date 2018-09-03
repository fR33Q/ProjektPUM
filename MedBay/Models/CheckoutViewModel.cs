using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedBay.Models
{
    public class CheckoutViewModel
    {
        public List<Cart> Carts { get; set; }
        public int TotalPrice { get; set; }
    }
}