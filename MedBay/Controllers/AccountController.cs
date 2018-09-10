using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using MedBay.Models;
using MedBay.DAL.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MedBay.App_Start;
using MedBay.DAL.IRepositories;

namespace MedBay.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ICustomerRepository customerRepository;
        private IProductRepository productRepository;
        public AccountController(ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                var role = _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
                return role;
            }
            private set
            {
                _roleManager = value;
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            var customerList = customerRepository.GetAllCustomers();
            var productList = productRepository.GetAllProducts();
            var categoryList = productRepository.GetAllCategories();
            AdminViewModel model = new AdminViewModel
            {
                CustomerList = customerList,
                ProductList = productList,
                Categories = categoryList
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddAdmin(string UserId)
        {
            string roleName = "Admin";
            
            if (UserId != null)
            {
                var roleresult = UserManager.AddToRole(UserId, roleName);

            }
            var customer = customerRepository.GetUserInformation(UserId);
            customerRepository.UpdateIsAdmin(customer.Id, true);

            return RedirectToAction("Admin", "Account");
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAdmin(string UserId)
        {
            string roleName = "Admin";

            var role = await RoleManager.FindByNameAsync(roleName);
            if (UserId != null)
            {
               var roleresult = UserManager.RemoveFromRole(UserId, roleName);
            
            }
            var customer = customerRepository.GetUserInformation(UserId);
            customerRepository.UpdateIsAdmin(customer.Id, false);

            return RedirectToAction("Admin", "Account");
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddAddress(ManageViewModel model)
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);
            var addressId = customerRepository.GetAddressIdForCustomer(customer.Id);
            Address adress = new Address
            {
                        Street = model.Address.Street,
                        Number = model.Address.Number,
                        City = model.Address.City,
                        PostalCode = model.Address.PostalCode,
                        Id = addressId
              
           };

            customerRepository.AddCustomerAddress(adress);

            return RedirectToAction("Index", "Manage");

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditCustomer(ManageViewModel model)
        {
            string currentUserId = User.Identity.GetUserId();
            var customer = customerRepository.GetUserInformation(currentUserId);

            Customer editedCustomer = new Customer
            {
                FirstName = model.Customer.FirstName,
                LastName = model.Customer.LastName,
                Email = model.Customer.Email,
                PhoneNumber = model.Customer.PhoneNumber,
                Id = customer.Id
            };

            customerRepository.EditCustomerInformation(editedCustomer);
            return RedirectToAction("Index", "Manage");

        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            const string role = "User";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                  
                    Customer customer = new Customer
                    {


                        FirstName = model.FirstName,
                        LastName =  model.LastName,
                        PhoneNumber = "",
                        Email = model.Email,
                        UserID = user.Id,
                        isAdmin = false,
                        Address = new Address
                        {
                            Street = "",
                            Number = "",
                            City = "",
                            PostalCode = ""
                        }


                    };
                    customerRepository.InsertCustomer(customer);


                    var roleResult = await UserManager.AddToRoleAsync(user.Id, role);
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First());
                        return View();
                    }
                    else
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                AddErrors(result);
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

     

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

      
    }
}