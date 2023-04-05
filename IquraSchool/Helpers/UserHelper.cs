using IquraSchool.Data;
using IquraSchool.Models;
using Microsoft.AspNetCore.Identity;

namespace IquraSchool.Helpers
{

    public class UserHelper
    {
        private readonly DbiquraSchoolContext _context;
        private readonly UserManager<User> _userManager;

        public UserHelper(DbiquraSchoolContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> AddUserStudent(Student student, int? year = null)
        {
            var existingUser = await _userManager.FindByEmailAsync(student.Email);
            if (existingUser == null)
            {
                await _context.SaveChangesAsync();
                User user = new User { Email = student.Email, UserName = student.Email, Year = year ?? 0 };
                user.StudentId = student.Id;
                var result = await _userManager.CreateAsync(user, "_Aa123456");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "student");
                }

                return result.Succeeded; // return a value indicating whether the operation succeeded
            }

            return false; // return a value indicating that the operation failed because a user with the same email already exists
        }


        public async Task AddUserTeacher(Teacher teacher)
        {
            var existingUser = await _userManager.FindByEmailAsync(teacher.Email);
            if (existingUser == null)
            {
                await _context.SaveChangesAsync();
                User user = new User { Email = teacher.Email, UserName = teacher.Email, Year = 2004 };
                user.TeacherId = teacher.Id;
                var result = await _userManager.CreateAsync(user, "_Aa123456");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "teacher");
                }
            }
        }
    }
}
