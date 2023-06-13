using BulkyRazor.Data;
using BulkyRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BulkyRazor.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        [BindProperties]
        private readonly ApplicationDbContext _context;

        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext db)
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
