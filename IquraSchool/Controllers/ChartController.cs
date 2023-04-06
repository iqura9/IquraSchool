using DocumentFormat.OpenXml.Wordprocessing;
using IquraSchool.Data;
using IquraSchool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IquraSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DbiquraSchoolContext _context;
        private readonly UserManager<User> _userManager;

        public ChartController(DbiquraSchoolContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet("JsonData")]
        public async Task<JsonResult> JsonData(int month, string academicYear)
        {
            var user = await _userManager.GetUserAsync(User);

            var yearParts = academicYear.Split('-');
            int startYear = int.Parse(yearParts[0]);
            int endYear = int.Parse(yearParts[1]);

            DateTime startDate = new DateTime(startYear, 9, 1);
            DateTime endDate = new DateTime(endYear, 8, 31);

            var gradesByCourse = _context.Grades
                .Include(g => g.Course.Subject)
                .Where(g => g.Student.Id == user.StudentId && g.Date.Month == month)
                .Where(g => g.Date >= startDate && g.Date <= endDate)
                .GroupBy(g => g.Course.Subject.Name)

                .Select(g => new object[] { g.Key, g.Average(x => x.Grade1) })
                .ToList();

            // Add the header row
            gradesByCourse.Insert(0, new object[] { "Course", "Average Grade" });

            return new JsonResult(gradesByCourse);
        }

        [HttpGet("JsonData1")]
        public async Task<JsonResult> JsonData1(int month, string academicYear)
        {
            var user = await _userManager.GetUserAsync(User);

            var yearParts = academicYear.Split('-');
            int startYear = int.Parse(yearParts[0]);
            int endYear = int.Parse(yearParts[1]);

            DateTime startDate = new DateTime(startYear, 9, 1);
            DateTime endDate = new DateTime(endYear, 8, 31);

            int previousStartYear = startYear - 1;
            int previousEndYear = endYear - 1;
            string previousAcademicYear = $"{previousStartYear}-{previousEndYear}";

            var allSubjects = _context.Subjects.Select(s => s.Name).ToList();

            var currentGradesByCourse = _context.Grades
                .Include(g => g.Course.Subject)
                .Where(g => g.Student.Id == user.StudentId && g.Date.Month == month)
                .Where(g => g.Date >= startDate && g.Date <= endDate)
                .GroupBy(g => g.Course.Subject.Name)
                .Select(g => new { SubjectName = g.Key, AverageGrade = g.Average(x => x.Grade1) })
                .ToList();

            var previousGradesByCourse = _context.Grades
                .Include(g => g.Course.Subject)
                .Where(g => g.Student.Id == user.StudentId && g.Date.Month == month)
                .Where(g => g.Date >= new DateTime(previousStartYear, month, 1) && g.Date <= new DateTime(previousEndYear, month, DateTime.DaysInMonth(previousEndYear, month)))
                .GroupBy(g => g.Course.Subject.Name)
                .Select(g => new { SubjectName = g.Key, AverageGrade = g.Average(x => x.Grade1) })
                .ToList();

            var gradesByCourse = allSubjects
                .GroupJoin(currentGradesByCourse,
                           x => x,
                           y => y.SubjectName,
                           (x, y) => new { SubjectName = x, CurrentAverageGrade = y.FirstOrDefault()?.AverageGrade ?? 0 })
                .GroupJoin(previousGradesByCourse,
                           x => x.SubjectName,
                           y => y.SubjectName,
                           (x, y) => new object[] { x.SubjectName, x.CurrentAverageGrade, y.FirstOrDefault()?.AverageGrade ?? 0 })
                .ToList();

            gradesByCourse.Insert(0, new object[] { "Курс", $"{academicYear} Середня Оцінка", $"{previousAcademicYear} Середня Оцінка" });

            return new JsonResult(gradesByCourse);
        }

    }
}
