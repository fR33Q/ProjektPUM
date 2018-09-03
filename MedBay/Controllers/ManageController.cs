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
    public class ManageController : Controller
    {
        private ICustomerRepository customerRepository;
        private IProductRepository productRepository;
        public ManageController(ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
        }

        // GET: Manage
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            if (customer != null)
            {
                var model = new ManageViewModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Customer = customer
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult AddProduct(AdminViewModel model)
        {
            productRepository.InsertProduct(model.Product);
            return RedirectToAction("Admin", "Account");
        }
    }
}