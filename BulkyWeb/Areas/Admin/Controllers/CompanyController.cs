
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            

        }
        public IActionResult Index()
        {   
            List<Company> prodList = _unitOfWork.Company.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }) ;
            return View(prodList);
        }
        public IActionResult Upsert(int? id)
        {

            
            if(id == null || id == 0)
            {
                    //create
                return View(new Company());
            }
            else
            {
                //update
                Company obj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(obj);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(Company obj)
        {

            if(ModelState.IsValid)
            {
               
                if(obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                }
                _unitOfWork.Save();
                TempData["success"] = "success";//like flash or popup
                return RedirectToAction("Index", "Company");

            }
            else
            {
                
                return View(obj);
            }
            
        }
     
     


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> prodList = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = prodList});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {   
            var obj = _unitOfWork.Company.Get(u => u.Id==id);
            if(obj == null)
            {
                return Json(new { success = false, message = "Erorr whieldel " });
            }
           
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "del-yeeted " });
        }
        #endregion
    }
}
