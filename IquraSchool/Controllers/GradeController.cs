using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Index(string? academicYear)
        {
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
            ViewData["CourseId"] = new SelectList(_context.Courses.Include(c => c.Subject).Where(c => c.TeacherId == 13), "Id", "Subject.Name");
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
            ViewData["CourseId"] = new SelectList(_context.Courses.Include(c => c.Subject), "Id", "Subject.Name", grade.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName", grade.StudentId);
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", grade.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", grade.StudentId);
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", grade.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", grade.StudentId);
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

        private bool GradeExists(int id)
        {
          return (_context.Grades?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
