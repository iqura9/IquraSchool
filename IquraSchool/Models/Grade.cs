using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models;

public partial class Grade
{
    public int Id { get; set; }
    [Display(Name = "Учень")]
    public int StudentId { get; set; }
    [Display(Name = "Оцінка")]
    [Range(1, 12, ErrorMessage = "Оцінка повинна бути в діапазоні від 1 до 12.")]
    public int? Grade1 { get; set; }
    [Display(Name = "Дата")]
    [DateLessThanOrEqualToToday(ErrorMessage = "Дата має бути меншою або дорівнювати сьогоднішній даті та часу")]
    public DateTime Date { get; set; }
    [Display(Name = "Предмет")]
    public int CourseId { get; set; }
    [Display(Name = "Відсутність")]
    public byte Absent { get; set; } = 0;
    [Display(Name = "Предмет")]
    public virtual Course Course { get; set; } = null!;
    [Display(Name = "Учень")]
    public virtual Student Student { get; set; } = null!;
}
