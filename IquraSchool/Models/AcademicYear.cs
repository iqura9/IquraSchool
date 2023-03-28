using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IquraSchool.Models;

public partial class AcademicYear
{
    public int Id { get; set; }
    [Display(Name = "Академічний рік (початок)")]
    [Range(1991, 2100, ErrorMessage = "Академічний рік повинний бути в діапазоні від 1991 до 2100.")]
    public int BeginYear { get; set; }
    [Display(Name = "Академічний рік (Кінець)")]
    public int EndYear { get; set; }
    [NotMapped]
    public string BeginEndYear => $"{BeginYear}-{EndYear}";
}
