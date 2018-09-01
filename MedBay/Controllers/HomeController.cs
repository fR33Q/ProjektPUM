using MedBay.DAL.Entity;
using MedBay.DAL.IRepositories;
using MedBay.DAL.Repositories;
using MedBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedBay.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository productRepository;
        public HomeController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ActionResult Index()
        {
            List<Product> products = productRepository.GetAllProducts();
            HomePageViewModel model = new HomePageViewModel
            {
                Products = products,
            };
           
            return View(model);
           
        }

    }
}