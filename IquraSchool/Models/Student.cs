using System;
using System.Collections.Generic;

namespace IquraSchool.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Image { get; set; }

    public int GroupId { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual Group Group { get; set; } = null!;
}
