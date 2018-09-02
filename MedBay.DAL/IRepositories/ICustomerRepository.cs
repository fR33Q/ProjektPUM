using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedBay.DAL.IRepositories
{
    public interface ICustomerRepository
    {
        Customer GetUserInformation(string userID);

        void InsertCustomer(Customer customer);
        List<Customer> GetAllCustomers();
        void AddCustomerAddress(Adress adress);
        int GetAddressIdForCustomer(int customerId);
        void EditCustomerInformation(Customer customer);

    }
}
