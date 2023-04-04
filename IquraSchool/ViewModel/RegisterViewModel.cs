using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataType = System.ComponentModel.DataAnnotations.DataType;
public enum RoleType
{
    Student,
    Teacher
}

namespace IquraSchool.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "П.І.Б")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Рік народження")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        //[NotMapped]
        [Display(Name = "Роль")]
        public RoleType Role { get; set; }
    }
}
