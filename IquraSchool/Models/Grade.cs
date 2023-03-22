using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models;

public partial class Grade
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    [Display(Name = "Оцінка")]
    public int? Grade1 { get; set; }
    [Display(Name = "Дата")]
    public DateTime Date { get; set; }
        
    public int CourseId { get; set; }
    [Display(Name = "Відсутність")]
    public byte Absent { get; set; }
    [Display(Name = "Предмет")]
    public virtual Course Course { get; set; } = null!;
    [Display(Name = "Учень")]
    public virtual Student Student { get; set; } = null!;
}
