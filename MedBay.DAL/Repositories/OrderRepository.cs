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
    public class OrderRepository : IOrderRepository
    {
        public string DeleteOrder(int id)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();
                Order order = db.Order.Find(id);

                db.Order.Attach(order);
                db.Order.Remove(order);
                db.SaveChanges();

                return "Cart was succesfully deleted";
            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public string EditOrderDetails(int clientId, Order order)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();

                Order o = (from x in db.Order
                    where x.CustomerID == clientId
                    select x).FirstOrDefault();

                o.CustomerID = order.CustomerID;
                o.FirstName = order.FirstName;
                o.LastName = order.LastName;
                o.Order_Price = order.Order_Price;
                o.PaymentMethodID = order.PaymentMethodID;
                o.PhontNumber = order.PhontNumber;
                o.ShipCity = order.ShipCity;
                o.TransportMethodID = order.TransportMethodID;
                o.ShipNumber = order.ShipNumber;
                o.ShipPostalCode = order.ShipNumber;
                o.ShipStreet = order.ShipStreet;
                db.SaveChanges();
                return "Order was succesfully updated";

            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public Order GetOrder(int clientId)
        {
            MedbayEntities db = new MedbayEntities();
            Order orderItem = (from x in db.Order
                               where x.CustomerID == clientId
                               select x).FirstOrDefault();
            return orderItem;
        }

        public List<TransportMethod> GetTransportMethodList()
        {
            MedbayEntities db = new MedbayEntities();
            List<TransportMethod> transportList = (from x in db.TransportMethod
                select x).ToList();
            return transportList;
        }


        public List<PaymentMethod> GetPaymentMethodList()
        {
            MedbayEntities db = new MedbayEntities();
            List<PaymentMethod> paymentList = (from x in db.PaymentMethod
                select x).ToList();
            return paymentList;

        }

        public TransportMethod GetTransportMethod(string transportMethodItem)
        {
            MedbayEntities db = new MedbayEntities();
            TransportMethod transport= (from x in db.TransportMethod
                where x.Name == transportMethodItem
                select x).FirstOrDefault();
            return transport;

        }

        public int GetPaymentMethodId(string paymentMethodItem)
        {
            MedbayEntities db = new MedbayEntities();
            int paymentId = (from x in db.PaymentMethod
                where x.Name == paymentMethodItem
                select x.Id).FirstOrDefault();
            return paymentId;
        }

        public bool InsertOrder(Order order)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();
                db.Order.Add(order);

                db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string UpdateOrder(int id, Order order)
        {
            try
            {
                MedbayEntities db = new MedbayEntities();

                Order o = db.Order.Find(id);

                o.CustomerID = order.CustomerID;
                o.FirstName = order.FirstName;
                o.LastName = order.LastName;
                o.Order_Price = order.Order_Price;
                o.PaymentMethodID = order.PaymentMethodID;
                o.PhontNumber = order.PhontNumber;
                o.ShipCity = order.ShipCity;
                o.ShipNumber = order.ShipNumber;
                o.ShipPostalCode = order.ShipNumber;
                o.ShipStreet = order.ShipStreet;
                db.SaveChanges();
                return "Order was succesfully updated";

            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }
    }
}
