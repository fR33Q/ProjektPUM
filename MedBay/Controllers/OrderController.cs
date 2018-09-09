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

        // GET: Order
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);
            var totalPrice = cartItems.Select(x => x.Cart_Price).Sum();
            var transportList = orderRepository.GetTransportMethodList();
            var paymentList = orderRepository.GetPaymentMethodList();       
            var orderItem = new Order();

            if (orderRepository.GetOrder(customer.Id) != null)
            {
                orderItem = orderRepository.GetOrder(customer.Id);
            }
            else
            {
                orderItem = new Order
                {
                    CustomerID = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhontNumber = customer.PhoneNumber,
                    ShipCity = customer.Address.City,
                    ShipStreet = customer.Address.Street,
                    ShipNumber = customer.Address.Number,
                    ShipPostalCode = customer.Address.PostalCode,
                    PaymentMethodID = paymentList.FirstOrDefault().Id,
                    TransportMethodID =  transportList.FirstOrDefault().Id,
                    Order_Price = totalPrice

                };
                orderRepository.InsertOrder(orderItem);
            }
            OrderViewModel model = new OrderViewModel()
            {
                OrderItem = orderItem,
                TransportList = transportList,
                PaymentList = paymentList,
                TotalPrice = totalPrice,
               
                
            };


            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditOrderInfoAndGoToSummary(OrderViewModel model)
        {

            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var transport = orderRepository.GetTransportMethod(model.TransportListItem);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);
            var cartTotalPrice = cartItems.Select(x => x.Cart_Price).Sum();
            var paymentId = orderRepository.GetPaymentMethodId(model.PaymentListItem);
            var totalPrice = cartTotalPrice + transport.Price;

            Order orderItem = new Order
            {
                CustomerID = customer.Id,
                FirstName = model.OrderItem.FirstName,
                LastName = model.OrderItem.LastName,
                PhontNumber = model.OrderItem.PhontNumber,
                ShipCity = model.OrderItem.ShipCity,
                ShipStreet = model.OrderItem.ShipStreet,
                ShipNumber = model.OrderItem.ShipNumber,
                ShipPostalCode = model.OrderItem.ShipPostalCode,
                PaymentMethodID = paymentId,
                TransportMethodID = transport.Id,
                Order_Price = totalPrice,
            };
            orderRepository.EditOrderDetails(customer.Id, orderItem);
            return RedirectToAction("Index", "Summary");
        }


        public ActionResult AbadonChanges(Order order)
        {
            orderRepository.DeleteOrder(order.Id);

            return RedirectToAction("Index", "Checkout");
        }
    }


}

