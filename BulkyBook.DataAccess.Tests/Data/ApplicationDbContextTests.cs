using BulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Tests.Data
{
    

    [TestFixture]
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            _context = MockDbGenerator.Create();
        }

        [TearDown]
        public void TearDown()
        {
            MockDbGenerator.Destroy(_context);
        }

        [Test]
        public void SeededCategories_ShouldBeCorrect()
        {
            // Arrange
            // ... Any necessary arrangement for this test.

            // Act: EF Core would run OnModelCreating and seed data here
            var categories = _context.Categories.ToListAsync().Result;

            // Assert
            Assert.AreEqual(4, categories.Count);
            Assert.IsTrue(categories.Exists(c => c.Name == "Action"));
            // ... Add more assertions for the other categories and their properties.

        }

        // ... other tests
    }

}
