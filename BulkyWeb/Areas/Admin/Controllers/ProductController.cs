
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {   
            List<Product> prodList = _unitOfWork.Product.GetAll(includeProps: "Category").ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }) ;
            return View(prodList);
        }
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if(id == null || id == 0)
            {
                    //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);//gives rendom name to file
                    string productPath = Path.  Combine(wwwRootPath, @"images\product");
                    if(!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + filename;

                }
                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "success";//like flash or popup
                return RedirectToAction("Index", "Product");

            }
            else
            {
                obj.CategoryList = _unitOfWork.Category
                 .GetAll().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });
                return View(obj);
            }
            
        }
     


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> prodList = _unitOfWork.Product.GetAll(includeProps: "Category").ToList();
            return Json(new {data = prodList});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {   
            var obj = _unitOfWork.Product.Get(u => u.Id==id);
            if(obj == null)
            {
                return Json(new { success = false, message = "Erorr whieldel " });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "del-yeeted " });
        }
        #endregion
    }
}
