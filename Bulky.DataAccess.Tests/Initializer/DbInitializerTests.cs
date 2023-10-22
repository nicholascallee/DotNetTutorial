using BulkyBook.DataAccess.Initializer;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BulkyBook.DataAccess.Tests.Initializer
{
    [TestFixture]
    public class DbInitializerTests
    {
        private DBInitializer _initializer;

        // Mocked Dependencies
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<ApplicationDbContext> _mockDbContext;

        [SetUp]
        public void Setup()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Email = "nicholascallee@gmail.com", UserName = "nicholascallee@gmail.com" }
            };

            // Mock DbContext
            var options = new DbContextOptions<ApplicationDbContext>();
            _mockDbContext = new Mock<ApplicationDbContext>(options);

            // Mock RoleManager
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            // Mock UserManager
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Mock RoleExistsAsync to always return false
            _mockRoleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);


            // Mock the app users
            var mockApplicationUsersList = new List<ApplicationUser>().AsQueryable();
            var _mockApplicationUsers = new Mock<DbSet<ApplicationUser>>();
            _mockApplicationUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(mockApplicationUsersList.Provider);
            _mockApplicationUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(mockApplicationUsersList.Expression);
            _mockApplicationUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(mockApplicationUsersList.ElementType);
            _mockApplicationUsers.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(mockApplicationUsersList.GetEnumerator());

            _mockDbContext.Setup(db => db.ApplicationUsers).Returns(_mockApplicationUsers.Object);

            // Initialize the DBInitializer with Mocked Dependencies
            _initializer = new DBInitializer(_mockUserManager.Object, _mockRoleManager.Object, _mockDbContext.Object);
        }

        [Test]
        public void RolesAreCreated_WhenNotExisting()
        {
            // Act
            _initializer.Initialize();

            // Assert
            _mockRoleManager.Verify(rm => rm.CreateAsync(It.Is<IdentityRole>(role => role.Name == SD.Role_Customer)), Times.Once);
            _mockRoleManager.Verify(rm => rm.CreateAsync(It.Is<IdentityRole>(role => role.Name == SD.Role_Employee)), Times.Once);
            _mockRoleManager.Verify(rm => rm.CreateAsync(It.Is<IdentityRole>(role => role.Name == SD.Role_Admin)), Times.Once);
            _mockRoleManager.Verify(rm => rm.CreateAsync(It.Is<IdentityRole>(role => role.Name == SD.Role_Company)), Times.Once);
        }

        [Test]
        public void UserIsCreated_WithCorrectData()
        {
            // Act
            _initializer.Initialize();

            // Assert
            _mockUserManager.Verify(um => um.CreateAsync(
                It.Is<ApplicationUser>(user =>
                    user.Email == "nicholascallee@gmail.com" &&
                    user.UserName == "nicholascallee@gmail.com" &&
                    user.Name == "Nicholas Allee" &&
                    user.PhoneNumber == "1234567890" &&
                    user.StreetAddress == "12345 way way" &&
                    user.State == "Missouri" &&
                    user.PostalCode == "64119" &&
                    user.City == "Maryville"),
                "REMOVED"),
            Times.Once);
        }

        [Test]
        public void Initialize_CreatesRoles_IfNotExisting()
        {
            // Act
            _initializer.Initialize();

            // Assert
            _mockRoleManager.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Exactly(4));
        }

        [Test]
        public void Initialize_CreatesUser_IfRoleCustomerDoesNotExist()
        {
            // Setup
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            _initializer.Initialize();

            // Assert
            _mockUserManager.Verify(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
