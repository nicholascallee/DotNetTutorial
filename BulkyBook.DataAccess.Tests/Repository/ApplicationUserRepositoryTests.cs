using BulkyBook.DataAccess.Repository;
using Moq;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [TestFixture]
    public class ApplicationUserRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private ProductRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _repository = new ProductRepository(_mockContext.Object);
            // Further setup for mocking DbSet etc.
        }

        [Test]
        public void Add_ValidProduct_IncreasesCount()
        {
            // Arrange
            //var product = new Product { /* initialization */ };

            // Act
            //_repository.Add(product);

            // Assert
            // assert that the mock context's products now include the added product.

        }

        // Additional tests for other methods...
    }

}
