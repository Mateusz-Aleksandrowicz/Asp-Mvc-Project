
using Asp_Net.DataAcess;
using Asp_Net.DataAcess.Repository.IRepository;
using Asp_Net.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp_Net.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();

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
                _unitOfWork.Category.Add(item);
                _unitOfWork.Save();
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
            var CategoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);   
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
                _unitOfWork.Category.Update(item);
                _unitOfWork.Save();
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

            var CategoryFromDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
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
            var item = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(item);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
