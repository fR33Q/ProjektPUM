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
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);
            var totalPrice = cartItems.Select(x => x.Cart_Price).Sum();
            CheckoutViewModel model = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalCartPrice = totalPrice
            };
            return View(model);
        }

        public ActionResult ClearCart()
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);
       

            cartRepository.DeleteCart(cartItems);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult UpdateTotalPrice()
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id); 
            double total = 0.0;
            try
            {

                foreach (var item in cartItems)
                {
                      total += item.Quantity * item.Cart_Price;
                }
                
            }
            catch (Exception) { total = 0; }


            return Json(new { d = String.Format("{0:c}", total) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QuanityChange(int type, int pId)
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var cartItems = cartRepository.GetOrdersInCart(customer.Id);
            var actualProduct = productRepository.GetProduct(pId);

            if (cartItems == null)
            {
                return Json(new { d = "0" });
            }

            var item = cartItems.FirstOrDefault(p => p.ProductID == pId);

                int quantity;
      
                switch (type)
                {
                    case 0:
                        item.Quantity--;
                        actualProduct.UnitsInStock++;
                        break;
                    case 1:
                        item.Quantity++;
                        actualProduct.UnitsInStock--;
                        break;
                    case -1:
                        actualProduct.UnitsInStock += item.Quantity;
                        item.Quantity = 0;
                        break;
                    default:
                        return Json(new { d = "0" });
                }
            

            if (item.Quantity == 0)
            {
                cartRepository.DeleteCartItem(item.Id);
                quantity = 0;
            }
            else
            {
                quantity = item.Quantity;
            }

            productRepository.UpdateProduct(actualProduct.ProductId, actualProduct);
            if (type != -1)
            {
                cartRepository.UpdateQuantity(item.Id, item.Quantity);
                cartRepository.UpdateTotalCartPrice(item.Id);
            }
            
            return Json(new { d = quantity });
        }
    }
}