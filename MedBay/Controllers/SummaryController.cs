using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedBay.DAL.Entity;
using MedBay.DAL.IRepositories;
using MedBay.Models;
using Microsoft.AspNet.Identity;

namespace MedBay.Controllers
{
    public class SummaryController : Controller
    {
        private ICustomerRepository customerRepository;
        private ICartRepository cartRepository;
        private IOrderRepository orderRepository;
        private IProductRepository productRepository;

        public SummaryController(IProductRepository productRepository,ICustomerRepository customerRepository, ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            this.productRepository = productRepository;
            this.customerRepository = customerRepository;
            this.cartRepository = cartRepository;
            this.orderRepository = orderRepository;
        }
        // GET: Summary
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);
            var orderItem = orderRepository.GetOrder(customer.Id);

            SummaryViewModel model = new SummaryViewModel()
            {
                OrderItem = orderItem,
                CartItems = cartItems,

            };
            return View(model);
        }

        public ActionResult EndOfShop(Order orderItems)
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);

            foreach (var item in cartItems)
            {
                var product = item.Product;
                product.UnitsInStock = product.UnitsInStock - item.Quantity;

                if (product.UnitsInStock <= 0)
                {
                    product.UnitsInStock = 0;
                }
                productRepository.UpdateProduct(product.ProductId, product);
            }
         
            orderRepository.DeleteOrder(orderItems.Id);
            cartRepository.DeleteCart(cartItems);

            return RedirectToAction("Index", "Home");
        }
    }
}