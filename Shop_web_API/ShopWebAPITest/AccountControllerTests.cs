using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Shop_web_API.Controllers;
using Shop_web_API.Models;
using Shop_web_API.Models.DTOs;


namespace ShopWebAPITest
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
          

            _userManagerMock = MockUserManager<ApplicationUser>();
            _signInManagerMock = MockSignInManager<ApplicationUser>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["JwtKey"]).Returns("TestKey");
            _configurationMock.Setup(c => c["JwtIssuer"]).Returns("TestIssuer");

            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Register_ValidModel_ReturnsOk()
        {
            // Arrange
            var model = new RegisterDTO { Email = "test@example.com", Password = "Test@1234", Role = "User" };
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), model.Role))
               .Returns(Task.FromResult(IdentityResult.Success)); // Properly configure Task result


            // Act
            var result = await _controller.Register(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully!", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
        }

        [Fact]
        public async Task Login_ValidModel_ReturnsToken()
        {
            // Arrange                         "test@example.com"             "Test@1234"
            var model = new LoginDTO { Email = "test@example.com", Password = "Test@1234" };
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(model.Email, model.Password, false, false))
                    .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _userManagerMock.Setup(um => um.FindByEmailAsync(model.Email))
                .ReturnsAsync(new ApplicationUser { UserName = model.Email, Email = model.Email });

            // Act
            var result = await _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value.GetType().GetProperty("token").GetValue(okResult.Value));
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<SignInManager<TUser>> MockSignInManager<TUser>() where TUser : class
        {
            var userManagerMock = MockUserManager<TUser>();
            return new Mock<SignInManager<TUser>>(userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<TUser>>(), null, null, null, null);
        }
    }
}