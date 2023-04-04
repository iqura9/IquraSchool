using System.Threading.Tasks;
using IquraSchool.Controllers;
using IquraSchool.Data;
using IquraSchool.Models;
using IquraSchool.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;


namespace IquraSchool.Tests.Controllers
{

    public class AccountControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<DbiquraSchoolContext> _mockContext;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DbiquraSchoolContext>().UseInMemoryDatabase(databaseName: "testDb").Options;
            _mockContext = new Mock<DbiquraSchoolContext>(dbContextOptions);
            _controller = new AccountController(_mockContext.Object, null, null);

        }


        [Fact]
        public async Task Register_AddsUserAndStudent_WhenModelStateIsValid_AndRoleIsStudent_AndGroupIdHasValue()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Email = "test@example.com",
                FullName = "Test User",
                Password = "P@ssword123",
                PasswordConfirm = "P@ssword123",
                Year = 2022,
                Role = RoleType.Student,
                GroupId = 1
            };
            var user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
            _mockUserManager.Setup(x => x.CreateAsync(user, model.Password)).ReturnsAsync(IdentityResult.Success);
            _mockSignInManager.Setup(x => x.SignInAsync(user, false, null)).Returns(Task.FromResult((object)null));
            int studentId = 1;
            _mockContext.Setup(x => x.Add(It.IsAny<Student>())).Callback<Student>(s => s.Id = studentId);
            _mockContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.Register(model);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
            Assert.Equal("Home", viewResult.ControllerName);

            _mockContext.Verify(x => x.Add(It.IsAny<Student>()), Times.Once);
            _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
            Assert.Equal(studentId, user.StudentId);
        }
    }
}