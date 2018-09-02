using MedBay.DAL.Entity;
using MedBay.DAL.IRepositories;
using MedBay.DAL.Repositories;
using MedBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace MedBay.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository productRepository;
        private ICartRepository cartRepository;
        private ICustomerRepository customerRepository;
        public HomeController(IProductRepository productRepository, ICartRepository cartRepository, ICustomerRepository customerRepository)
        {
            this.productRepository = productRepository;
            this.cartRepository = cartRepository;
            this.customerRepository = customerRepository;
        }

        public ActionResult Index()
        {
            List<Cart> cart;
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            List<Product> products = productRepository.GetAllProducts();
            
            if (customer != null)
            {
                cart = cartRepository.GetOrdersInCart(customer.Id);
                HomePageViewModel model = new HomePageViewModel
                {
                    Products = products,
                    Cart = cart
                };

                return View(model);
            }
            else
            {
                HomePageViewModel model = new HomePageViewModel
                {
                    Products = products,
                    Cart = null
                    
                };

                return View(model);
            }
           
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
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
           // var product = productRepository.GetProduct(id);
            Cart cart = new Cart
            {
                ProductID = id,
                CustomerID = customer.Id,
                Quantity = 1
            };
            cartRepository.InsertCart(cart);
            return RedirectToAction("Index");
        }

    }
}