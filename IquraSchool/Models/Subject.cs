using System;
using System.Collections.Generic;

namespace IquraSchool.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Course> Courses { get; } = new List<Course>();
}
