using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Order> GetAllOrdersByUser(bool includeItems, string userName)
        {
            try
            {
                if (includeItems)
                    return _context.Orders
                        .Where(o => o.User.UserName == userName)
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .ToList();

                return _context.Orders
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            try
            {
                if(includeItems)
                    return _context.Orders
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .ToList();

                return _context.Orders
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return null;
            }
        }

        public Order GetOrderById(string userName, int id)
        {
            try
            {
                return _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .SingleOrDefault(o => o.Id == id && o.User.UserName == userName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Couldn't get order: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                return _context.Products
                        .OrderBy(p => p.Title)
                        .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try
            {
                return _context.Products
                        .Where(p => p.Category == category)
                        .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save: {ex}");
                return false;
            }
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public void AddOrder(Order newOrder)
        {
            //TODO: convert new products to lookup of product
            foreach (var item in newOrder.Items)
            {
                item.Product = _context.Products.Find(item.Product.Id);
            }

            AddEntity(newOrder);
        }
    }
}
