
using Asp_Net.DataAcess;
using Asp_Net.DataAcess.Repository.IRepository;
using Asp_Net.Models;
using Asp_Net.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asp_Net.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnviroment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnviroment)
        {
            _unitOfWork = unitOfWork;
            _HostEnviroment = hostEnviroment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u=> u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM item, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _HostEnviroment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(rootPath, @"img\products");
                    var newPath = Path.GetExtension(file.FileName);

                    if(item.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(rootPath, item.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fs = new FileStream(Path.Combine(uploads, fileName + newPath), FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                    item.Product.ImageUrl = @"\img\products\" + fileName + newPath;
                }
                if(item.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(item.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(item.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
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

            var ProductFromDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return Json(new { data = productList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var item = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(item);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
        #endregion
    }
}