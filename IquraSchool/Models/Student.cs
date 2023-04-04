using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models;

public partial class Student
{
    public int Id { get; set; }
    [Display(Name = "П.І.Б")]
    public string FullName { get; set; } = null!;
    [Display(Name = "Електронна пошта")]
    public string Email { get; set; } = null!;
    [Display(Name = "Фото")]
    public string? Image { get; set; }
    [Display(Name = "Клас")]
    public int GroupId { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();
    [Display(Name = "Клас")]
    public virtual Group Group { get; set; } = null!;
}
