using IquraSchool.Data;
using IquraSchool.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.RawSQLExtensions.Extensions;
using System.ComponentModel.DataAnnotations;
using System;

namespace IquraSchool.Controllers
{

    public class RequestController : Controller
    {

        private readonly DbiquraSchoolContext _context;
        private readonly UserManager<User> _userManager;

        public RequestController(DbiquraSchoolContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? queryOneId)
        {
            return View();
        }

        public async Task<IActionResult> QueryOne(int? queryOneId)
        {
            string query = @"SELECT s.""Id"", s.""Email"", s.""FullName"", s.""Image"", g.""Id"" AS ""GroupId""
                     FROM ""Student"" AS s
                     JOIN ""Group"" AS g ON s.""GroupId"" = g.""Id""";

            if (queryOneId.HasValue)
            {
                query += $@" WHERE g.""Id"" = {queryOneId}";
            }
            var Data = await _context.Students.FromSqlRaw(query).ToListAsync();

            ViewBag.Groups = new SelectList(_context.Groups.OrderByDescending(s => s.Name), "Id", "Name", queryOneId);

            return View(Data);
        }

        public class StudentGradeViewModel
        {
            public int Id { get; set; }
            public string Email { get; set; }
            [Display(Name = "П.І.Б")]
            public string FullName { get; set; }
            public int GroupId { get; set; }
            public string Image { get; set; }
            [Display(Name = "Середня оцінка")]
            public Decimal AverageGrade { get; set; }
        }
        public async Task<IActionResult> QueryTwo(int? subjectId = 1)
        {
            string query = $@"SELECT s.""Id"", s.""Email"",s.""FullName"", s.""GroupId"", s.""Image"", AVG(g.""Grade"") AS ""AverageGrade""
                            FROM ""Student"" AS s
                            JOIN ""Grade"" AS g ON s.""Id"" = g.""StudentId""
                            JOIN ""Course"" AS c ON g.""CourseId"" = c.""Id""
                            JOIN ""Subject"" AS sub ON c.""SubjectId"" = sub.""Id""
                            WHERE sub.""Id"" = {subjectId}
                            GROUP BY s.""Id"";";

            var yourData = await _context.Database.SqlQuery<StudentGradeViewModel>(query).ToListAsync();

            ViewBag.Subject = new SelectList(_context.Subjects.OrderByDescending(s => s.Name), "Id", "Name", subjectId);

            return View(yourData);
        }

        public class Schedule_InfoViewModel
        {
            public int Id { get; set; }
            [Display(Name = "Клас")]
            public string GroupName { get; set; }
            [Display(Name = "Номер уроку")]
            public int LessonNumber { get; set; }
            [Display(Name = "Назва предмету")]
            public string SubjectName { get; set; }
            [Display(Name = "П.І.Б вчителя")]
            public string TeacherName { get; set; }
        }
        public async Task<IActionResult> QueryThree(int? groupId = 11, int? dayListId = 0)
        {
            string query = $@"SELECT g.""Name"" AS ""GroupName"", si.""LessonNumber"", sub.""Name"" AS ""SubjectName"", t.""FullName"" AS ""TeacherName""
                            FROM ""Schedule_Info"" AS si
                            JOIN ""Group"" AS g ON si.""GroupId"" = g.""Id""
                            JOIN ""Course"" AS c ON si.""CourseId"" = c.""Id""
                            JOIN ""Subject"" AS sub ON c.""SubjectId"" = sub.""Id""
                            JOIN ""Teacher"" AS t ON c.""TeacherId"" = t.""Id""
                            WHERE si.""DayOfTheWeek"" = {dayListId} -- 2 - відповідає середі
                              AND g.""Id"" = {groupId}
                            ORDER BY si.""LessonNumber"";";

            var yourData = await _context.Database.SqlQuery<Schedule_InfoViewModel>(query).ToListAsync();

            var daylist = new List<SelectListItem>();
            List<string> days = new List<string>() { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця" };
            for (int i = 0; i <= 4; i++)
            {
                daylist.Add(new SelectListItem { Value = i.ToString(), Text = days[i] });
            }
            ViewBag.Daylist = new SelectList(daylist, "Value", "Text", dayListId);

            ViewBag.Groups = new SelectList(_context.Groups.OrderByDescending(s => s.Name), "Id", "Name", groupId);

            return View(yourData);
        }
        public async Task<IActionResult> QueryFour(decimal? grade = 0)
        {
            string query = $@"SELECT s.""Id"", s.""Email"",s.""FullName"", s.""GroupId"", s.""Image"", AVG(g.""Grade"") AS ""AverageGrade""
                            FROM ""Student"" AS s
                            JOIN ""Grade"" AS g ON s.""Id"" = g.""StudentId""
                            GROUP BY s.""Id""
                            HAVING AVG(g.""Grade"") > {grade};";

            var yourData = await _context.Database.SqlQuery<StudentGradeViewModel>(query).ToListAsync();

            var gradeList = new List<SelectListItem>();
            for (decimal i = 0; i <= 12*2; i ++)
            {
                gradeList.Add(new SelectListItem { Value = (i/2).ToString(), Text = (i / 2).ToString() });
            }
            ViewBag.GradeList = new SelectList(gradeList, "Value", "Text", grade);

            return View(yourData);
        }
        public async Task<IActionResult> QueryFive(string? fullname = "")
        {
            try
            {
                string query = $@"SELECT g.""Name"" AS ""GroupName"", si.""LessonNumber"", si.""DayOfTheWeek"", sub.""Name"" AS ""SubjectName""
                            FROM ""Schedule_Info"" AS si
                            JOIN ""Course"" AS c ON si.""CourseId"" = c.""Id""
                            JOIN ""Subject"" AS sub ON c.""SubjectId"" = sub.""Id""
                            JOIN ""Teacher"" AS t ON c.""TeacherId"" = t.""Id""
                            JOIN ""Group"" AS g ON si.""GroupId"" = g.""Id""
                            WHERE t.""FullName"" = {fullname};";

                var yourData = query.Length > 0 ? await _context.Database.SqlQuery(query).ToListAsync() : null;
            }
            catch {}
            ViewBag.Teacher = new SelectList(_context.Teachers.OrderByDescending(s => s.FullName), "Id", "FullName");

            return View();
        }
        public class TeacherViewModel
        {
            public int Id { get; set; }
            [Display(Name = "П.І.Б")]
            public string FullName { get; set; } = null!;
            [Display(Name = "Електронна пошта")]
            public string Email { get; set; } = null!;
            [Display(Name = "Фото")]
            public string? Image { get; set; }

            [Display(Name = "Предмет")]
            public string Name { get; set; }
        }
        public async Task<IActionResult> QueryOneHard(int? teacherId = 1)
        {
            string query = $@"SELECT t.*,sub.""Name""
                            FROM ""Teacher"" as t
                            INNER JOIN ""Course"" AS co ON co.""TeacherId"" = t.""Id""
                            INNER JOIN ""Subject"" AS sub ON sub.""Id"" = co.""SubjectId""
                            WHERE t.""Id"" <> {teacherId} AND NOT EXISTS (
                                  SELECT co2.""SubjectId""
                                  FROM ""Course"" as co2
                                  WHERE co2.""TeacherId"" = {teacherId}
                                  EXCEPT
                                  SELECT co.""SubjectId""
                                  FROM ""Course"" as co
                                  WHERE co.""TeacherId"" = t.""Id""
                               )
                            order by t.""Id"";";



            var yourData = await _context.Database.SqlQuery<TeacherViewModel>(query).ToListAsync();

            ViewBag.Teacher = new SelectList(_context.Teachers.OrderByDescending(s => s.FullName), "Id", "FullName", teacherId);

            return View(yourData);
        }

        public async Task<IActionResult> QueryTwoHard(int? teacherId = 1)
        {
            string query = $@"SELECT t.*, sub.""Name""
                            FROM ""Teacher"" AS t
                            INNER JOIN ""Course"" AS co ON co.""TeacherId"" = t.""Id""
                            INNER JOIN ""Subject"" AS sub ON sub.""Id"" = co.""SubjectId""
                            WHERE co.""TeacherId"" <> {teacherId}
                              AND NOT exists(
                                               (SELECT su.""Id""
                                                FROM ""Subject"" AS su
                                                WHERE su.""Id"" IN
                                                    (SELECT co2.""SubjectId""
                                                     FROM ""Course"" AS co2
                                                     WHERE co2.""TeacherId""=co.""TeacherId"") )
                                             EXCEPT (
                                                       (SELECT su.""Id""
                                                        FROM ""Subject"" AS su
                                                        WHERE su.""Id"" in
                                                            (SELECT o.""SubjectId""
                                                             FROM ""Course"" AS o
                                                             WHERE o.""TeacherId""= {teacherId}))))
                              AND NOT exists((
                                                (SELECT su.""Id""
                                                 FROM ""Subject"" AS su
                                                 WHERE su.""Id"" in
                                                     (SELECT o.""SubjectId""
                                                      FROM ""Course"" AS o
                                                      WHERE o.""TeacherId""= {teacherId})))
                                             EXCEPT
                                               (SELECT su.""Id""
                                                FROM ""Subject"" AS su
                                                WHERE su.""Id"" IN
                                                    (SELECT co2.""SubjectId""
                                                     FROM ""Course"" AS co2
                                                     WHERE co2.""TeacherId""=co.""TeacherId"") ));";



            var yourData = await _context.Database.SqlQuery<TeacherViewModel>(query).ToListAsync();

            ViewBag.Teacher = new SelectList(_context.Teachers.OrderByDescending(s => s.FullName), "Id", "FullName", teacherId);

            return View(yourData);
        }
        public async Task<IActionResult> QueryThreeHard(int? teacherId = 1)
        {
            string query = $@"select t.*, sub.""Name"" 
                            from ""Teacher"" as t
                            INNER JOIN ""Course"" as co ON co.""TeacherId"" = t.""Id""
                            INNER JOIN ""Subject"" as sub ON co.""SubjectId"" = sub.""Id""
                                         where co.""TeacherId"" <> {teacherId} and not exists(
                                                (
                                                    (select sub1.""Id"" from ""Subject"" as sub1 where sub1.""Id"" in (select co2.""SubjectId"" from ""Course"" as co2 where co2.""TeacherId""= {teacherId}))
                                                )
                                                except
                                                (
                                                    select sub1.""Id"" from ""Subject"" as sub1 where sub1.""Id"" IN (select co3.""SubjectId"" from ""Course"" as co3 where co3.""TeacherId""=co.""TeacherId"")
                                                )
                                            )
                                          and EXISTS(
                                                (
                                                    (select sub1.""Id"" from ""Subject"" as sub1 where sub1.""Id"" IN (select co3.""SubjectId"" from ""Course"" as co3 where co3.""TeacherId""=co.""TeacherId""))
                                                )
                                                except
                                                (
                                                        (select sub1.""Id"" from ""Subject"" as sub1 where sub1.""Id"" in (select co2.""SubjectId"" from ""Course"" as co2 where co2.""TeacherId""={teacherId}))
                                            )
                                            );";



            var yourData = await _context.Database.SqlQuery<TeacherViewModel>(query).ToListAsync();

            ViewBag.Teacher = new SelectList(_context.Teachers.OrderByDescending(s => s.FullName), "Id", "FullName", teacherId);

            return View(yourData);
        }

        // GET: RequestController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RequestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
