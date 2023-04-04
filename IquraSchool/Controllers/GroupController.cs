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

namespace IquraSchool.Controllers
{
    public class GroupController : Controller
    {
        private readonly DbiquraSchoolContext _context;

        public GroupController(DbiquraSchoolContext context)
        {
            _context = context;
        }


        // GET: Group
        
        public async Task<IActionResult> Index()
        {
            var dbiquraSchoolContext = _context.Groups.Include(m => m.HeadTeacher);
            return View(await dbiquraSchoolContext.ToListAsync());
        }

        // GET: Group/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(m => m.HeadTeacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            //return View(group);
            return RedirectToAction("Index", "Student", new { id = group.Id, name = group.Name });
        }

        // GET: Group/Create
        public IActionResult Create()
        {
            ViewData["HeadTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName");
            return View();
        }

        // POST: Group/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,HeadTeacherId")] Group group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HeadTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", group.HeadTeacherId);
            return View(group);
        }

        // GET: Group/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            ViewData["HeadTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", group.HeadTeacherId);
            return View(group);
        }

        // POST: Group/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,HeadTeacherId")] Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.Id))
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
            ViewData["HeadTeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", group.HeadTeacherId);
            return View(group);
        }

        // GET: Group/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(m => m.HeadTeacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'DbiquraSchoolContext.Groups'  is null.");
            }
            var group = await _context.Groups
                .FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
          return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
