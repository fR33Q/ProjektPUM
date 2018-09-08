using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedBay.DAL.Entity;
using System.Data.Entity;
using MedBay.DAL.IRepositories;

namespace MedBay.DAL.Repositories
{
    public class CartRepository : ICartRepository
    {
        public bool InsertCart(Cart cart)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();
                db.Cart.Add(cart);
                
                db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string DeleteCartItem(int id)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();
                Cart cart = db.Cart.Find(id);

                db.Cart.Attach(cart);
                db.Cart.Remove(cart);
                db.SaveChanges();

                return "Cart was succesfully deleted";
            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public string DeleteCart(List<Cart> cartItems)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();
                foreach (var cartItem in cartItems)
                {
                    Cart cart = db.Cart.Find(cartItem.Id);

                    if (cart != null)
                    {
                        db.Cart.Attach(cart);
                        db.Cart.Remove(cart);
                    }
                    db.SaveChanges();
                }
        

                return "Cart was succesfully deleted";
            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public string UpdateCart(int id, Cart cart)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();

                //Fetch object from db
                Cart p = db.Cart.Find(id);


                p.CustomerID = cart.CustomerID;
                p.Quantity = cart.Quantity;
                p.ProductID = cart.ProductID;
                p.Product = cart.Product;         
                db.SaveChanges();
                return "Cart was succesfully updated";

            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public void UpdateQuantity(int id, int quantity)
        {
            MedbayEntities db = new MedbayEntities();
            Cart p = db.Cart.Find(id);
            p.Quantity = quantity;

            db.SaveChanges();
        }

        public List<Cart> GetOrdersInCart(int clientId)
        {
            MedbayEntities db = new MedbayEntities();
            List<Cart> orders = (from x in db.Cart
                                 where x.CustomerID == clientId
                                 select x).ToList();
            return orders;
        }

        public Cart GetCartItem(int cartItemId)
        {
            MedbayEntities db = new MedbayEntities();
            Cart cartItem = db.Cart.Find(cartItemId);
            return cartItem;
        }
    }
}
