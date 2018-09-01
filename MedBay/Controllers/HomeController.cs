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
        private ICartRepository cartRepository;
        public HomeController(IProductRepository productRepository, ICartRepository cartRepository)
        {
            this.productRepository = productRepository;
            this.cartRepository = cartRepository;
        }

        public ActionResult Index()
        {
            List<Product> products = productRepository.GetAllProducts();
           // Cart cart = cartRepository.GetOrdersInCart(clientId);
            HomePageViewModel model = new HomePageViewModel
            {
                Products = products,
            };
           
            return View(model);
           
        }

        public ActionResult Category(string catName)
        {
            List<Product> products;
            var categoryId = productRepository.GetCategoryId(catName);
            if (catName == "")
            {
               products = productRepository.GetAllProducts();
            }
            else
            {
               products = productRepository.GetProductsByCategory(categoryId);
            }
           
            HomePageViewModel model = new HomePageViewModel
            {
                Products = products,
            };

            return View("Index",model);
        }
        public ActionResult AddToCart(int id)
        {
            var product = productRepository.GetProduct(id);
            Cart cart = new Cart
            {
                Product = product,
            };
            cartRepository.InsertCart(cart);
            return RedirectToAction("Index");
        }

    }
}