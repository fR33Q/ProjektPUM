using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedBay.DAL.IRepositories;
using Microsoft.AspNet.Identity;

namespace MedBay.Controllers
{
    public class OrderController : Controller
    {
        private IProductRepository productRepository;
        private ICustomerRepository customerRepository;
        private ICartRepository cartRepository;
        private IOrderRepository orderRepository;

        public OrderController(IProductRepository productRepository, ICustomerRepository customerRepository, ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
            this.cartRepository = cartRepository;
            this.orderRepository = orderRepository;
        }

        // GET: Purchase
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);
            var totalPrice = cartItems.Select(x => x.Cart_Price).Sum();
            return View("OrderView");
        }
    }
}
