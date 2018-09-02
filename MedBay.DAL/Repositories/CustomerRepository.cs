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

        public void AddCustomerAddress(Adress adress)
        {
            MedbayEntities db = new MedbayEntities();
            var addressFromDb = (from x in db.Adress
                        where x.Id == adress.Id
                        select x).FirstOrDefault();
            addressFromDb.Street = adress.Street;
            addressFromDb.Number = adress.Number;
            addressFromDb.PostalCode = adress.PostalCode;
            addressFromDb.City = adress.City;
            db.SaveChanges();
        }

        public int GetAddressIdForCustomer(int customerId)
        {
            MedbayEntities db = new MedbayEntities();
            var addressId = (from x in db.Customer
                        where x.Id == customerId
                        select x.AdressID).FirstOrDefault();
            return addressId;
        }

        public void EditCustomerInformation(Customer customer)
        {
            MedbayEntities db = new MedbayEntities();
            var customerFromDb = (from x in db.Customer
                                 where x.Id == customer.Id
                                 select x).FirstOrDefault();

            customerFromDb.FirstName = customer.FirstName;
            customerFromDb.LastName = customer.LastName;
            customerFromDb.PhoneNumber = customer.PhoneNumber;
            customerFromDb.Email = customer.Email;
            db.SaveChanges();
        }
    }
}
