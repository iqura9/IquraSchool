using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models;

public partial class ScheduleInfo
{
    public int Id { get; set; }
    [Range(1, 8, ErrorMessage = "Діапазон від 1 до 8 предметів на день")]
    [Display(Name = "Номер уроку")]
    public int LessonNumber { get; set; }
    [Range(0, 4, ErrorMessage = "Помилка вводу дня тижня")]
    [Display(Name = "День тижня")]
    public int DayOfTheWeek { get; set; }
    [Display(Name = "Клас")]
    public int GroupId { get; set; }
    [Display(Name = "Предмет")]
    public int CourseId { get; set; }
    [Display(Name = "Посилання на урок")]
    public string? Link { get; set; }
    [Display(Name = "Предмет")]
    public virtual Course Course { get; set; } = null!;
    [Display(Name = "Клас")]
    public virtual Group Group { get; set; } = null!;
}
