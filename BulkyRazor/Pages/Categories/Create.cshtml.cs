using BulkyRazor.Data;
using BulkyRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyRazor.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {   
        private readonly ApplicationDbContext _context;

        public Category Category { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _context = db; 
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            _context.Categories.Add(Category);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
