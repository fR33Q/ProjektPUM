using MedBay.DAL.Entity;
using MedBay.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedBay.DAL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Customer GetUserInformation(string userID)
        {
            MedbayEntities db = new MedbayEntities();
            var info = (from x in db.Customer
                        where x.UserID == userID
                        select x).FirstOrDefault();
            return info;
        }
        public void InsertCustomer(Customer customer)
        {
            MedbayEntities db = new MedbayEntities();
            db.Customer.Add(customer);
            db.SaveChanges();
        }

        public List<Customer> GetAllCustomers()
        {
            MedbayEntities db = new MedbayEntities();
            var customerList = (from x in db.Customer

                        select x).ToList();
            return customerList;
        }
    }
}
