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
        bool InsertCart(Cart cart);
        string DeleteCartItem(int id);
        string DeleteCart(List<Cart> cartItems);
        string UpdateCart(int id, Cart cart);
        void UpdateQuantity(int id, int quantity);
        List<Cart> GetOrdersInCart(int clientId);
        Cart GetCartItem(int cartItemId);
        void UpdateTotalCartPrice(int id);

    }
}
