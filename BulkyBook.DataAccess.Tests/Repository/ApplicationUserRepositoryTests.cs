using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [TestFixture]
    public class ApplicationUserRepositoryTests
    {
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _context;
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            // Create mock of IWebHostEnvironment
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.ContentRootPath).Returns("C:\\Development\\visualStudioProjects\\Bulky\\BulkyWeb");

            // Create startup instance
            var startup = new StartupTest(mockEnvironment.Object);
            startup.ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Real TEst repository for integration tests
            _unitOfWork = new UnitOfWork(_context);
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
    }

}
