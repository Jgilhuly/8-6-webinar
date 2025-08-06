using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using RestaurantOps.Legacy.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantOps.Legacy.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantOpsContext _context;

        public OrderRepository(RestaurantOpsContext context)
        {
            _context = context;
        }

        public Order Create(int tableId)
        {
            var order = new Order 
            { 
                TableId = tableId, 
                CreatedAt = DateTime.UtcNow,
                Status = "Open"
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order? GetCurrentByTable(int tableId)
        {
            return _context.Orders
                .Include(o => o.Lines)
                .Where(o => o.TableId == tableId && o.Status == "Open")
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();
        }

        public Order? GetById(int orderId)
        {
            return _context.Orders
                .Include(o => o.Lines)
                .FirstOrDefault(o => o.OrderId == orderId);
        }

        public void AddLine(int orderId, int menuItemId, int qty, decimal price)
        {
            var menuItem = _context.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemId);
            var orderLine = new OrderLine
            {
                OrderId = orderId,
                MenuItemId = menuItemId,
                MenuItemName = menuItem?.Name ?? string.Empty,
                Quantity = qty,
                PriceEach = price
            };
            _context.OrderLines.Add(orderLine);
            _context.SaveChanges();
        }

        public void CloseOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = "Closed";
                order.ClosedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        public void SubmitOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = "Submitted";
                _context.SaveChanges();
            }
        }

        public IEnumerable<OrderLine> GetLines(int orderId)
        {
            return _context.OrderLines
                .Where(ol => ol.OrderId == orderId)
                .ToList();
        }
    }
} 