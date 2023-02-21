using System;
using System.Collections.Generic;

namespace IquraSchool.Models;

public partial class ScheduleInfo
{
    public int Id { get; set; }

    public int LessonNumber { get; set; }

    public int DayOfTheWeek { get; set; }

    public int GroupId { get; set; }

    public int CourseId { get; set; }

    public string? Link { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}
