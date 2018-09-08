using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedBay.DAL.Entity;

namespace MedBay.Models
{
    public class OrderViewModel
    {
        public string ShipFirstName { get; set; }
        public string ShipLastName { get; set; }
        public string ShipPhoneNumber { get; set; }
        public string ShipEmail { get; set; }
        public Address ShipAddress { get; set; }
        public Customer Customer { get; set; }
        public List<Cart> CartItems { get; set; }
        public Order OrderItem { get; set; }
        public int TotalPrice { get; set; }
    }
}