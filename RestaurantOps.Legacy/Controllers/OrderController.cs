using Microsoft.AspNetCore.Mvc;
using System.Linq;
using RestaurantOps.Legacy.Data;

namespace RestaurantOps.Legacy.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly ITableRepository _tableRepo;

        public OrderController(IOrderRepository orderRepo, IMenuRepository menuRepo, ITableRepository tableRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
            _tableRepo = tableRepo;
        }

        public IActionResult Details(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.MenuItems = _menuRepo.GetAll();
            return View(order);
        }

        [HttpPost]
        public IActionResult AddItem(int orderId, int menuItemId, int quantity)
        {
            var menuItem = _menuRepo.GetAll().First(mi => mi.MenuItemId == menuItemId);
            _orderRepo.AddLine(orderId, menuItemId, quantity, menuItem.Price);
            return RedirectToAction("Details", new { id = orderId });
        }

        [HttpPost]
        public IActionResult Close(int orderId)
        {
            var order = _orderRepo.GetById(orderId);
            if (order != null)
            {
                _orderRepo.CloseOrder(orderId);
                _tableRepo.UpdateOccupied(order.TableId, false);
            }
            return RedirectToAction("Index", "Tables");
        }

        [HttpPost]
        public IActionResult Submit(int orderId)
        {
            _orderRepo.SubmitOrder(orderId);
            return RedirectToAction("Details", new { id = orderId });
        }
    }
} 