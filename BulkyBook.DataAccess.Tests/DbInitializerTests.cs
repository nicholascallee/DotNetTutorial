using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Initializer;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BulkyBook.DataAccess.Tests
{
    [TestFixture]
    public class DbInitializerTests
    {
        private DBInitializer _initializer;

        // Mocked Dependencies
        private Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<ApplicationDbContext> _mockDbContext;







        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }






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

            // ... Mock any methods you need. For instance:
            _mockRoleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);


            var mockUsersDbSet = GetQueryableMockDbSet(users);
            _mockDbContext.Setup(db => db.ApplicationUsers).Returns(mockUsersDbSet);



            // Initialize the DBInitializer with Mocked Dependencies
            _initializer = new DBInitializer(_mockUserManager.Object, _mockRoleManager.Object, _mockDbContext.Object);

        }

        [TearDown]
        public void TearDown()
        {
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
