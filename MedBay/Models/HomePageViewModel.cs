using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedBay.Models
{
    public class HomePageViewModel
    {
        public List<Product> Products { get; set; }
        public List<Cart> Cart { get; set; }
    }
}