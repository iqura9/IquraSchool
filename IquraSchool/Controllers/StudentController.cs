using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IquraSchool.Models;
using IquraSchool.Data;
using ClosedXML.Excel;

namespace IquraSchool.Controllers
{
    public class StudentController : Controller
    {
        private readonly DbiquraSchoolContext _context;

        public StudentController(DbiquraSchoolContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.GroupId = id;
            ViewBag.GroupName = name;
            var dbiquraSchoolContext = _context.Students.Include(s => s.Group).Where(s => s.Group.Id == id);
            return View(await dbiquraSchoolContext.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Image,GroupId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", student.GroupId);
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", student.GroupId);
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Image,GroupId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Name", student.GroupId);
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'DbiquraSchoolContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if(fileExcel == null)
                {
                    return BadRequest("���� �� �������� ���� ������");
                }
                using(var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                {
                    await fileExcel.CopyToAsync(stream);
                    using(XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                    {
                        //�������� ��� ����� (�����)
                        foreach(IXLWorksheet worksheet in workBook.Worksheets)
                        {
                            Group newGroup;
                            var c = (from g in _context.Groups
                                     where g.Name.Contains(worksheet.Name)
                                     select g).ToList();
                            if(c.Count > 0)
                            {
                                newGroup = c[0];
                            }
                            else
                            {
                                newGroup = new Group();
                                newGroup.Name = worksheet.Name;
                                _context.Groups.Add(newGroup);
                            }
                            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                            {
                                try
                                {
                                    Student student = new Student();
                                    student.FullName = row.Cell(1).Value.ToString();
                                    student.Email = row.Cell(2).Value.ToString();
                                    student.Image = row.Cell(3).Value.ToString();
                                    student.Group = newGroup;
                                    _context.Students.Add(student);
                                }
                                catch(Exception e) { 
                                    
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
