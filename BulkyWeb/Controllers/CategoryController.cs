using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
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
            //retrieving data
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {  
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "ORder cant match name");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test aint valid");
                //since no key, binded to all in validation summary
            }


            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "success";//like flash or popup
                return RedirectToAction("Index", "Category");
            }
            return View();
            
        }

        public IActionResult Edit(int? id)
        {  
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id == id);//can retrieve without primary key
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {   
            if(ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "success edited";//like flash or popup
                return RedirectToAction("Index", "Category");
            }
            return RedirectToAction("Index", "Category");
        }
        public IActionResult Delete(int? id)
        {       
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int?id)
        {
            Category? category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "success del-yeet";//like flash or popup
            return RedirectToAction("Index", "Category");
        }
    }
}
