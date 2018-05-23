using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);

        IEnumerable<Order> GetAllOrdersByUser(bool includeItems, string userName);
        IEnumerable<Order> GetAllOrders(bool includeItems);
        Order GetOrderById(string userName, int id);

        bool SaveAll();
        void AddEntity(object model);
        void AddOrder(Order newOrder);
    }
}