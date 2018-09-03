using MedBay.DAL.IRepositories;
using MedBay.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedBay.Controllers
{
    public class CheckoutController : Controller
    {
        private IProductRepository productRepository;
        private ICustomerRepository customerRepository;
        private ICartRepository cartRepository;
        public CheckoutController(IProductRepository productRepository, ICustomerRepository customerRepository, ICartRepository cartRepository)
        {
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
            this.cartRepository = cartRepository;
        }

        // GET: Checkout
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var carts = cartRepository.GetOrdersInCart(customer.Id);
            var totalPrice = carts.Select(x => x.Cart_Price).Sum();
            CheckoutViewModel model = new CheckoutViewModel
            {
                Carts = carts,
                TotalPrice = totalPrice
            };
            return View(model);
        }
    }
}