using System.ComponentModel.DataAnnotations;

namespace StudentsMVC.Models
{
    public class Student
    {
        [Display(Name = "Ідентифікатор")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть ім'я")]
        [Display(Name = "Ім'я")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть прізвище")]
        [Display(Name = "Прізвище")]
        public required string Surname { get; set; }

        [Range(15, 100, ErrorMessage = "Вік має бути від 15 до 100 років")]
        [Display(Name = "Вік")]
        public int Age { get; set; }

        [Display(Name = "Середній бал (GPA)")]
        public double GPA { get; set; }
    }
}