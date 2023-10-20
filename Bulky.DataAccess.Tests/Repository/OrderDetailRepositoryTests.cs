using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [TestFixture]
    public class OrderDetailRepositoryTests
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
        public void Update_ValidOrderDetail_UpdatesOrderDetail()
        {
            var orderDetailToUpdate = _unitOfWork.OrderDetail.Get(u => u.Id == 1);
            orderDetailToUpdate.Price = 1;  // Placeholder: Adjust the property name and value as required
            _unitOfWork.OrderDetail.Update(orderDetailToUpdate);
            _unitOfWork.Save();

            var updatedOrderDetail = _unitOfWork.OrderDetail.Get(u => u.Id == 1);
            Assert.That(updatedOrderDetail.Price, Is.EqualTo(1));  // Placeholder: Adjust the property name as required
        }

        // Additional tests for other methods...
    }

}
