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
        public async Task<JsonResult> JsonData(int month)
        {
            var user = await _userManager.GetUserAsync(User);
            var gradesByCourse = _context.Grades
                .Include(g => g.Course.Subject)
                .Where(g => g.Student.Id == user.StudentId && g.Date.Month == month)
                .GroupBy(g => g.Course.Subject.Name)
                
                .Select(g => new object[] { g.Key, g.Average(x => x.Grade1) })
                .ToList();

            // Add the header row
            gradesByCourse.Insert(0, new object[] { "Course", "Average Grade" });

            return new JsonResult(gradesByCourse);
        }

    }
}
