using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IquraSchool.Models;
using IquraSchool.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;


namespace IquraSchool.Controllers
{
    public class GradeController : Controller
    {
        private readonly DbiquraSchoolContext _context;
        private readonly UserManager<User> _userManager;

        public GradeController(DbiquraSchoolContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Grade
        public async Task<IActionResult> Index(int? id, string? academicYear)
        {
            //Progress by userId
            if (id != null)
            {
                var user = await _userManager.GetUserAsync(User);
                return RedirectToAction("Progress", "Grade", new { id = user.StudentId });
            }


            //Grade
            DateTime startDate = new DateTime(DateTime.Today.Year - 23, 9, 1);
            DateTime endDate = new DateTime(DateTime.Today.Year + 1, 8, 31);
            
            ViewBag.academicYears = new SelectList(_context.AcademicYears, "BeginEndYear", "BeginEndYear", academicYear);

            if (!string.IsNullOrEmpty(academicYear))
            {
                // Parse the selected academic year from the dropdown list
                var yearParts = academicYear.Split('-');
                int startYear = int.Parse(yearParts[0]);
                int endYear = int.Parse(yearParts[1]);

                // Set the start and end dates based on the selected academic year
                startDate = new DateTime(startYear, 9, 1);
                endDate = new DateTime(endYear, 8, 31);
            }

            var dbiquraSchoolContext = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course.Subject)
                .Include(g => g.Course.Teacher)
                .Where(g => g.Date >= startDate && g.Date <= endDate);

            return View(await dbiquraSchoolContext.ToListAsync());
        }


        // GET: Grade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .Include(g => g.Course)
                .Include(g => g.Course.Subject)
                .Include(g => g.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // GET: Grade/Create
        [Authorize(Roles = "admin, teacher")]
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole(Role.Teacher))
            {
                var user = await _userManager.GetUserAsync(User);
                ViewData["CourseId"] = new SelectList(_context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.Subject)
                    .OrderBy(c => c.Subject.Name)
                    .Where(c => c.Teacher.Id == user.TeacherId)
                    .Select(c => new {
                        Id = c.Id,
                        Name = c.Subject.Name + " - " + c.Teacher.FullName
                    }), "Id", "Name");
            }
            else
            {
                ViewData["CourseId"] = new SelectList(_context.Courses
                    .Include(c => c.Subject)
                    .Include(c => c.Teacher)
                    .OrderBy(c => c.Subject.Name)
                    .Select(c => new {
                        Id = c.Id,
                        Name = c.Subject.Name + " - " + c.Teacher.FullName
                    }), "Id", "Name");
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName");
            return View();
        }

        // POST: Grade/Create
        [HttpPost]
        [Authorize(Roles = "admin, teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,Grade1,Date,CourseId,Absent")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            else if (User.IsInRole(Role.Teacher))
            {
                var user = await _userManager.GetUserAsync(User);
                ViewData["CourseId"] = new SelectList(_context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.Subject)
                    .OrderBy(c => c.Subject.Name)
                    .Where(c => c.Teacher.Id == user.TeacherId)
                    .Select(c => new {
                        Id = c.Id,
                        Name = c.Subject.Name + " - " + c.Teacher.FullName
                    }), "Id", "Name");
            }
            else
            {
                ViewData["CourseId"] = new SelectList(_context.Courses
                    .Include(c => c.Subject)
                    .Include(c => c.Teacher)
                    .OrderBy(c => c.Subject.Name)
                    .Select(c => new {
                        Id = c.Id,
                        Name = c.Subject.Name + " - " + c.Teacher.FullName
                    }), "Id", "Name");
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName");
            return View(grade);
        }

        // GET: Grade/Edit/5
        [Authorize(Roles = "admin, teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses.Include(c => c.Subject).OrderBy(s => s.Subject.Name), "Id", "Subject.Name");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName");
            return View(grade);
        }

        // POST: Grade/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin, teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Grade1,Date,CourseId,Absent")] Grade grade)
        {
            if (id != grade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(grade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses.Include(c => c.Subject).OrderBy(s => s.Subject.Name), "Id", "Subject.Name", grade.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName", grade.StudentId);
            return View(grade);
        }

        // GET: Grade/Delete/5
        [Authorize(Roles = "admin, teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .Include(g => g.Course)
                .Include(g => g.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: Grade/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Grades == null)
            {
                return Problem("Entity set 'DbiquraSchoolContext.Grades'  is null.");
            }
            var grade = await _context.Grades.FindAsync(id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Grade/Progress/5

        [Authorize(Roles = "student")]
        public async Task<IActionResult> Progress(int? id, int? month, string? academicYear)
        {
            
            if(month == null && academicYear == null)
            {
                return RedirectToAction("Progress", "Grade", new {id = id, month = DateTime.Now.Month, academicYear = "2022-2023"});
            }

            if (id == null)
            {
                return BadRequest("Student ID is required.");
            }

            DateTime startDate = new DateTime(DateTime.Today.Year - 23, 9, 1);
            DateTime endDate = new DateTime(DateTime.Today.Year + 1, 8, 31);

            ViewBag.academicYears = new SelectList(_context.AcademicYears, "BeginEndYear", "BeginEndYear", academicYear);

            if (!string.IsNullOrEmpty(academicYear))
            {
                // Parse the selected academic year from the dropdown list
                var yearParts = academicYear.Split('-');
                int startYear = int.Parse(yearParts[0]);
                int endYear = int.Parse(yearParts[1]);

                // Set the start and end dates based on the selected academic year
                startDate = new DateTime(startYear, 9, 1);
                endDate = new DateTime(endYear, 8, 31);
            }

            var grades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course.Subject)
                .Include(g => g.Course.Teacher)
                .Where(g => g.Student.Id == id && (month == null || g.Date.Month == month))
                .Where(g => g.Date >= startDate && g.Date <= endDate)
                .ToListAsync();


            List<string> months = new List<string>();
            DateTime date = new DateTime(DateTime.Now.Year, 9, 1); // September 1st of the current year
            for (int i = 0; i < 12; i++)
            {
                months.Add(date.AddMonths(i).ToString("MMMM"));
            }

            ViewBag.Months = new SelectList(months.Select((m, i) => new { Value = (i + 9) % 12 != 0 ? (i + 9) % 12 : 12, Text = m }), "Value", "Text", month);
            ViewBag.Subjects = await _context.Subjects.Distinct().OrderBy(m => m.Name).Select(m => m.Name).ToListAsync();

  
            return View("Progress", grades);
        }


        public async Task<ActionResult> Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var grades = await _context.Grades
                    .Include(g => g.Student)
                    .Include(g => g.Course.Subject)
                    .Include(g => g.Course.Teacher)
                    .Where(g => g.Student.Id == 407)
                    .OrderBy(g => g.Date)
                    .ToListAsync();

                // Group grades by month
                var gradesByMonth = grades.GroupBy(g => g.Date.Month);

                foreach (var monthGroup in gradesByMonth)
                {
                    var worksheet = workbook.Worksheets.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Key));

                    worksheet.Cell("A1").Value = "Учень";
                    worksheet.Cell("B1").Value = "Предмет";
                    worksheet.Cell("C1").Value = "Викладач";
                    worksheet.Cell("D1").Value = "Оцінка";
                    worksheet.Cell("E1").Value = "Присутність";
                    worksheet.Cell("F1").Value = "Дата";
                    

                    worksheet.Row(1).Style.Font.Bold = true;
                    worksheet.Column(1).Width = 40;
                    worksheet.Column(2).Width = 21;
                    worksheet.Column(3).Width = 40;
                    worksheet.Column(4).Width = 10;
                    worksheet.Column(5).Width = 10;
                    worksheet.Column(6).Width = 15;

                    var row = 2;
                    List<string> absentList = new List<string>() { "", "Н", "ПП" };
                    // Add grades for the current month
                    foreach (var grade in monthGroup)
                    {
                        worksheet.Cell(row, 1).Value = grade.Student.FullName;
                        worksheet.Cell(row, 2).Value = grade.Course.Subject.Name;
                        worksheet.Cell(row, 3).Value = grade.Course.Teacher.FullName;
                        worksheet.Cell(row, 4).Value = grade.Grade1;
                        worksheet.Cell(row, 5).Value = absentList[grade.Absent];
                        worksheet.Cell(row, 6).Value = grade.Date.ToString("yyyy-MM-dd");

                        row++;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"grades_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        private bool GradeExists(int id)
        {
          return (_context.Grades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
