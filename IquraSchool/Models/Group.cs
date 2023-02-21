using System;
using System.Collections.Generic;

namespace IquraSchool.Models;

public partial class Group
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int HeadTeacherId { get; set; }

    public virtual Teacher HeadTeacher { get; set; } = null!;

    public virtual ICollection<ScheduleInfo> ScheduleInfos { get; } = new List<ScheduleInfo>();

    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
