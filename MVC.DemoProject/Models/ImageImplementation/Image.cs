using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.DemoProject.Models.ImageImplementation
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; } = null;

        [NotMapped]
        [Display(Name= "Choose Image")]
        public IFormFile ImageFile { get; set; }
    }
}
