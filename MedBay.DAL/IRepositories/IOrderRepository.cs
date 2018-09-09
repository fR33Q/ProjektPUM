using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedBay.DAL.IRepositories
{
    public interface IOrderRepository
    {
        bool InsertOrder(Order order);
        string DeleteOrder(int id);
        string UpdateOrder(int id, Order order);
        string EditOrderDetails(int clientId, Order order);
        Order GetOrder(int clientId);
        List<TransportMethod> GetTransportMethodList();
        List<PaymentMethod> GetPaymentMethodList();
        TransportMethod GetTransportMethod(string transportMethodItem);
        int GetPaymentMethodId(string paymentMethodItem);
    }
}
