using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraSchool.Models;

public partial class Course
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please select a subject.")]
    public int SubjectId { get; set; }

    [Required(ErrorMessage = "Please select a teacher.")]
    public int TeacherId { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual ICollection<ScheduleInfo> ScheduleInfos { get; } = new List<ScheduleInfo>();

    public virtual Subject Subject { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
