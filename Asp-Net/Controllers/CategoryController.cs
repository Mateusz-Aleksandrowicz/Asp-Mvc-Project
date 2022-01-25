
using Asp_Net.DataAcess;
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
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0 )
            {
                return NotFound();
            }

           // var CategoryFromDb = _db.Categories.Find(id);
            var CategoryFromDbFirst = _db.Categories.FirstOrDefault(x => x.Name == "id");   
            if(CategoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(CategoryFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category item)
        {

            if (item.Name == item.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name");
            }

            if (ModelState.IsValid)
            {
                _db.Categories.Update(item);
                _db.SaveChanges();
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var CategoryFromDb = _db.Categories.Find(id);
            if (CategoryFromDb == null)
            {
                return NotFound();
            } 

            return View(CategoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var item = _db.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(item);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
