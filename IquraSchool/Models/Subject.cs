using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models;

public partial class Subject
{
    public int Id { get; set; }
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;
    [Display(Name = "Опис")]
    public string? Description { get; set; }

    public virtual ICollection<Course> Courses { get; } = new List<Course>();
}
