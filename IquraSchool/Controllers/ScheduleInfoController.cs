using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IquraSchool.Models;
using IquraSchool.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NuGet.DependencyResolver;

namespace IquraSchool.Controllers
{

    public class ScheduleInfoController : Controller
    {
        private readonly DbiquraSchoolContext _context;
        private readonly UserManager<User> _userManager;

        public ScheduleInfoController(DbiquraSchoolContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        List<string> daysOfWeek = new List<string> { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };
        // GET: ScheduleInfo
        public async Task<IActionResult> Index(int? id,string? view)
        {
            ViewBag.DayOfTheWeeks = daysOfWeek;
            IOrderedQueryable<ScheduleInfo> dbiquraSchoolContext = _context.ScheduleInfos
                .Include(s => s.Course)
                .Include(s => s.Course.Subject)
                .Include(s => s.Course.Teacher)
                .Include(s => s.Group)
                .OrderBy(s => s.DayOfTheWeek)
                .ThenBy(s => s.LessonNumber);
            
            if(id == null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user.TeacherId != null) id = user.TeacherId;
                else if (user.StudentId != null) id = user.StudentId;
            }
            
            
            if (_context.Students.Any(s => s.Id == id))
            {
                var student = _context.Students.Include(s => s.Group).Where(s => s.Id == id).FirstOrDefault();
                ViewBag.Name = student.FullName;
                ViewBag.Group = student.Group.Name;
                dbiquraSchoolContext = dbiquraSchoolContext
                    .Where(s => s.Group.Students.Any(s => s.Id == id))
                    .OrderBy(s => s.DayOfTheWeek)
                    .ThenBy(s => s.LessonNumber);
            }
            else if (_context.Teachers.Any(s => s.Id == id))
            {
               var teacher = _context.Teachers.Where(s => s.Id == id).FirstOrDefault();
               ViewBag.Name = teacher.FullName;
                dbiquraSchoolContext = dbiquraSchoolContext.Where(s => s.Course.Teacher.Id == id)
                    .OrderBy(s => s.DayOfTheWeek)
                    .ThenBy(s => s.LessonNumber);
            }

            if (view == "grid")
            {
                return View("Grid",await dbiquraSchoolContext.ToListAsync());
            }

            return View(await dbiquraSchoolContext.ToListAsync());
        }

        // GET: ScheduleInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ScheduleInfos == null)
            {
                return NotFound();
            }

            var scheduleInfo = await _context.ScheduleInfos
                .Include(s => s.Course)
                .Include(s => s.Course.Subject)
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleInfo == null)
            {
                return NotFound();  
            }

            return View(scheduleInfo);
        }

        // GET: ScheduleInfo/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            
            //Excelent code
            ViewData["CourseId"] = new SelectList(_context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .OrderBy(c => c.Subject.Name)
                .Select(c => new {
                    Id = c.Id,
                    Name = c.Subject.Name + " - " + c.Teacher.FullName
                }), "Id", "Name");

            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            ViewData["DayOfTheWeek"] = new SelectList(daysOfWeek.Select((d,i) => new {Value = i, Text = d}), "Value", "Text");
           
            return View();
        }

        // POST: ScheduleInfo/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LessonNumber,DayOfTheWeek,GroupId,CourseId,Link")] ScheduleInfo scheduleInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .OrderBy(c => c.Subject.Name)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Subject.Name + " - " + c.Teacher.FullName
                }), "Id", "Name", scheduleInfo.CourseId);

            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", scheduleInfo.GroupId);
            ViewData["DayOfTheWeek"] = new SelectList(daysOfWeek.Select((d, i) => new { Value = i, Text = d }), "Value", "Text", scheduleInfo.DayOfTheWeek);

            return View(scheduleInfo);
        }

        // GET: ScheduleInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ScheduleInfos == null)
            {
                return NotFound();
            }

            var scheduleInfo = await _context.ScheduleInfos.FindAsync(id);
            if (scheduleInfo == null)
            {
                return NotFound();
            }

            ViewData["CourseId"] = new SelectList(_context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .OrderBy(c => c.Subject.Name)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Subject.Name + " - " + c.Teacher.FullName
                }), "Id", "Name", scheduleInfo.CourseId);

            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", scheduleInfo.GroupId);
            ViewData["DayOfTheWeek"] = new SelectList(daysOfWeek.Select((d, i) => new { Value = i, Text = d }), "Value", "Text", scheduleInfo.DayOfTheWeek);
            return View(scheduleInfo);
        }

        // POST: ScheduleInfo/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LessonNumber,DayOfTheWeek,GroupId,CourseId,Link")] ScheduleInfo scheduleInfo)
        {
            if (id != scheduleInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduleInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleInfoExists(scheduleInfo.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .OrderBy(c => c.Subject.Name)
                .Select(c => new
                {
                    Id = c.Id,
                    Name = c.Subject.Name + " - " + c.Teacher.FullName
                }), "Id", "Name", scheduleInfo.CourseId);

            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", scheduleInfo.GroupId);
            ViewData["DayOfTheWeek"] = new SelectList(daysOfWeek.Select((d, i) => new { Value = i, Text = d }), "Value", "Text", scheduleInfo.DayOfTheWeek);
            return View(scheduleInfo);
        }

        // GET: ScheduleInfo/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ScheduleInfos == null)
            {
                return NotFound();
            }

            var scheduleInfo = await _context.ScheduleInfos
                .Include(s => s.Course)
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleInfo == null)
            {
                return NotFound();
            }

            return View(scheduleInfo);
        }

        // POST: ScheduleInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ScheduleInfos == null)
            {
                return Problem("Entity set 'DbiquraSchoolContext.ScheduleInfos'  is null.");
            }
            var scheduleInfo = await _context.ScheduleInfos.FindAsync(id);
            if (scheduleInfo != null)
            {
                _context.ScheduleInfos.Remove(scheduleInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleInfoExists(int id)
        {
          return (_context.ScheduleInfos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
