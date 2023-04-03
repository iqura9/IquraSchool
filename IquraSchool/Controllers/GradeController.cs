using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IquraSchool.Models;
using IquraSchool.Data;


namespace IquraSchool.Controllers
{
    public class GradeController : Controller
    {
        private readonly DbiquraSchoolContext _context;

        public GradeController(DbiquraSchoolContext context)
        {
            _context = context;
        }

        // GET: Grade
        public async Task<IActionResult> Index(int? id, string? academicYear)
        {
            //Progress by userId
            if (id != null) return RedirectToAction("Progress","Grade", new { id = id });


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
        public IActionResult Create()
        {
            // #TODO replace 13 with real teacherId
            //ViewData["CourseId"] = new SelectList(_context.Courses.Include(c => c.Subject).Where(c => c.TeacherId == 13), "Id", "Subject.Name");
            ViewData["CourseId"] = new SelectList(_context.Courses.Include(c => c.Subject).OrderBy(s => s.Subject.Name), "Id", "Subject.Name");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName");
            return View();
        }

        // POST: Grade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,Grade1,Date,CourseId,Absent")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses.Include(c => c.Subject).OrderBy(s => s.Subject.Name), "Id", "Subject.Name");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName");
            return View(grade);
        }

        // GET: Grade/Edit/5
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
        private bool GradeExists(int id)
        {
          return (_context.Grades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
