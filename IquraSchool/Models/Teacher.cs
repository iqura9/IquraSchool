using System;
using System.Collections.Generic;

namespace IquraSchool.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Image { get; set; }

    public virtual ICollection<Course> Courses { get; } = new List<Course>();

    public virtual Group? Group { get; set; }
}
