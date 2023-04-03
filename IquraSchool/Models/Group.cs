using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models;

public partial class Group
{
    public int Id { get; set; }
    [Display(Name = "Клас")]
    public string Name { get; set; } = null!;
    [Display(Name = "Класний керівник")]
    public int? HeadTeacherId { get; set; }
    [Display(Name = "Класний керівник")]
    public virtual Teacher HeadTeacher { get; set; } = null;

    public virtual ICollection<ScheduleInfo> ScheduleInfos { get; } = new List<ScheduleInfo>();
    
    public virtual ICollection<Student> Students { get; } = new List<Student>();
}
