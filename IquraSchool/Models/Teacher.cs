﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models;
[Display(Name = "Вчитель")]
public partial class Teacher
{
    public int Id { get; set; }
    [Display(Name = "П.І.Б")]
    public string FullName { get; set; } = null!;
    [Display(Name = "Електронна пошта")]
    public string Email { get; set; } = null!;
    [Display(Name = "Фото")]
    public string? Image { get; set; }

    public virtual ICollection<Course> Courses { get; } = new List<Course>();

    public virtual Group? Group { get; set; }
}
