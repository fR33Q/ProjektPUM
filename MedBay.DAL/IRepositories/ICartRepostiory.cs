using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedBay.DAL.IRepositories
{
    public interface ICartRepository
    {
        string InsertCart(Cart cart);
        string DeleteCart(int id);
        string UpdateCart(int id, Cart cart);
        void UpdateQuantity(int id, int quantity);
        List<Cart> GetOrdersInCart(int clientId);
       
    }
}
