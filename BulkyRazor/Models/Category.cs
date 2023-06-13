using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyRazor.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(0, 500)]
        public int DisplayOrder { get; set; }
    }
}
