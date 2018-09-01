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
       
    }
}
