using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IquraSchool.Models;

namespace IquraSchool.Controllers
{
    public class ScheduleInfoController : Controller
    {
        private readonly DbiquraSchoolContext _context;

        public ScheduleInfoController(DbiquraSchoolContext context)
        {
            _context = context;
        }

        // GET: ScheduleInfo
        public async Task<IActionResult> Index()
        {
            var dbiquraSchoolContext = _context.ScheduleInfos.Include(s => s.Course).Include(s => s.Group);
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
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scheduleInfo == null)
            {
                return NotFound();
            }

            return View(scheduleInfo);
        }

        // GET: ScheduleInfo/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id");
            return View();
        }

        // POST: ScheduleInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LessonNumber,DayOfTheWeek,GroupId,CourseId,Link")] ScheduleInfo scheduleInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", scheduleInfo.CourseId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", scheduleInfo.GroupId);
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", scheduleInfo.CourseId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", scheduleInfo.GroupId);
            return View(scheduleInfo);
        }

        // POST: ScheduleInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", scheduleInfo.CourseId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", scheduleInfo.GroupId);
            return View(scheduleInfo);
        }

        // GET: ScheduleInfo/Delete/5
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
