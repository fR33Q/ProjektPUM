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
            List<Product> products = productRepository.GetAllProducts();
           // List<Cart> cart = cartRepository.GetOrdersInCart(clientId);
            HomePageViewModel model = new HomePageViewModel
            {
                Products = products,
                //Cart = cart
            };

       
            //register z ręki dla sprawdzenia

            //var userStore = new UserStore<IdentityUser>();
            //userStore.Context.Database.Connection.ConnectionString =
            //    System.Configuration.ConfigurationManager.ConnectionStrings["MedbayEntitiesAccount"].ConnectionString;
            //var manager = new UserManager<IdentityUser>(userStore);
            //var user = new IdentityUser { UserName = "Test" };

    
            //IdentityResult result =  manager.Create(user, "1234567");

            //if (result.Succeeded)
            //{

            //    Customer customer = new Customer
            //    {


            //        FirstName = "Test",
            //        LastName = "Test",
            //        PhoneNumber = "",
            //        Email = "",
            //        UserID = user.Id,
            //        Adress = new Adress
            //        {
            //            Street = "",
            //            Number = "",
            //            City = "",
            //            PostalCode = ""
            //        }


            //    };


            //    customerRepository.InsertCustomer(customer);
            //}




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