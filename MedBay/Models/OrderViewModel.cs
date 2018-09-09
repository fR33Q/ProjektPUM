using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedBay.DAL.Entity;

namespace MedBay.Models
{
    public class OrderViewModel
    {
        public Order OrderItem { get; set; }
        public List<TransportMethod> TransportList { get; set; }
        public List<PaymentMethod> PaymentList { get; set; }
        public SelectList selekcja { get; set; }
        public string TransportListItem { get; set; }
        public string PaymentListItem { get; set; }

        public int TotalPrice { get; set; }
    }
}