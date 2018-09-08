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
        private List<Cart> cart;
        private List<Product> productList;
        public HomeController(IProductRepository productRepository, ICartRepository cartRepository, ICustomerRepository customerRepository)
        {
            this.productRepository = productRepository;
            this.cartRepository = cartRepository;
            this.customerRepository = customerRepository;
        }

        public ActionResult Index(string catName)
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);


            var products = GetProducts(catName);
            
            if (customer != null)
            {
                cart = cartRepository.GetOrdersInCart(customer.Id);
                var totalPrice = cart.Select(x => x.Cart_Price).Sum();
                var totalCount = cart.Select(x => x.Quantity).Sum();
                HomePageViewModel model = new HomePageViewModel
                {
                    Products = products,
                    Cart = cart,
                    TotalPrice = totalPrice,
                    TotalCount = totalCount
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

        public ActionResult AddToCart(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            var product = productRepository.GetProduct(id);
            var customer = customerRepository.GetUserInformation(currentUserId);
           // var totalPrice = 1 * product.Price;
            Cart cartItem = new Cart
            {
                ProductID = id,
                CustomerID = customer.Id,
                Quantity = 1,
                Cart_Price = product.Price,
                TotalCartPrice = product.Price // to do usuniecia w sumie

            };

            // comment lines - nie tutaj ale zostawiam do wykorzystania w summaryView 
            if (product.UnitsInStock > 0)
            {
                cartRepository.InsertCart(cartItem);
               // product.UnitsInStock--;
              //  productRepository.UpdateProduct(product.ProductId, product);
            }
            else
            {
                //TODO: Informacja o braku produktów na stanie.
            }



            return RedirectToAction("Index");
        }

        public ActionResult DeleteCart(int id)
        {
            Cart cartItem = new Cart();
            cartItem = cartRepository.GetCartItem(id);       
            cartRepository.DeleteCartItem(cartItem.Id);
            //do summaryView
            //var product = productRepository.GetProduct(cartItem.ProductID);
            //product.UnitsInStock++;
            //productRepository.UpdateProduct(cartItem.ProductID, product);
            return RedirectToAction("Index");
        }


        private List<Product> GetProducts(string categoryName)
        {
            
            var categoryId = productRepository.GetCategoryId(categoryName);
            if (string.IsNullOrEmpty(categoryName))
            {
                productList = productRepository.GetAllProducts();
            }
            else
            {
                productList = productRepository.GetProductsByCategory(categoryId);

            }
          
            return productList;

        }
        

    }
}