using System.ComponentModel.DataAnnotations;

namespace MVC.DemoProject
{
    public class productViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public IFormFile Image { get; set; }
    }
}
