using System;
using System.Collections.Generic;

namespace IquraSchool.Models;

public partial class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int? Grade1 { get; set; }

    public DateTime Date { get; set; }

    public int CourseId { get; set; }

    public byte Absent { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
