using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataType = System.ComponentModel.DataAnnotations.DataType;

public static class Role
{
    public static readonly string Admin = "admin";
    public static readonly string Student = "student";
    public static readonly string Teacher = "teacher";
}

public enum RoleType
{
    Admin,
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
        [Display(Name = "Клас")]
        public int? GroupId { get; set; }
    }
}
