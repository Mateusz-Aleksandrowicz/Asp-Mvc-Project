using Asp_Net.Data;
using Asp_Net.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Net.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db; 
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories;

            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category item)
        {

            if(item.Name == item.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","The DisplayOrder cannot exactly match the Name");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Add(item);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }
    }
}
