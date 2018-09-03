using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedBay.Models
{
    public class AdminViewModel
    {
        public List<Customer> CustomerList { get; set; }
        public List<Product> ProductList { get; set; }
        public Product Product { get; set; }
        public List<Category> Categories { get;set; }
    }
}