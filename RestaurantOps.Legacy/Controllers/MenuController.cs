using Microsoft.AspNetCore.Mvc;
using RestaurantOps.Legacy.Data;
using RestaurantOps.Legacy.Models;

namespace RestaurantOps.Legacy.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuRepository _repo;

        public MenuController(IMenuRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var items = _repo.GetAll();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MenuItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MenuItem item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }
            _repo.Add(item);
            return RedirectToAction(nameof(Index));
        }
    }
} 