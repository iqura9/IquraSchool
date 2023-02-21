using System;
using System.Collections.Generic;

namespace IquraSchool.Models;

public partial class Course
{
    public int Id { get; set; }

    public int SubjectId { get; set; }

    public int TeacherId { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual ICollection<ScheduleInfo> ScheduleInfos { get; } = new List<ScheduleInfo>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
