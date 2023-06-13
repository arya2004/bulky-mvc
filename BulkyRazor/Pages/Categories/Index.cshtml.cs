using BulkyRazor.Data;
using BulkyRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyRazor.Pages.Categories
{
    public class IndexModel : PageModel
    {   
        private readonly ApplicationDbContext _context;
        public List<Category> Categories { get; set; }
        public IndexModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
           Categories = _context.Categories.ToList();
        }
    }
}
