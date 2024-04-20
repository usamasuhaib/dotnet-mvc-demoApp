using System.ComponentModel.DataAnnotations;

namespace MVC.DemoProject.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student Name")]

        public string Name { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "EmailAddress")]
        public string Email { get; set; }

        [Display(Name = "MobileNumber")]
        public string Mobile { get; set; }

    }
}
