using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BulkyBook.DataAccess.Tests.Repository
{
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Microsoft.EntityFrameworkCore;
    using BulkyBook.DataAccess.Repository.IRepository;
    using BulkyBook.DataAccess.Repository;
    using BulkyBook.DataAccess.Data;
    using Microsoft.AspNetCore.Hosting;

    [TestFixture]
    public class ProductRepositoryTests
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
        public void GetAll_ReturnsAllProducts()
        {
            var results = _unitOfWork.Product.GetAll().ToList();
            // Depending on how your DBInitializer seeds data, adjust the expected count
            Assert.That(results.Count, Is.EqualTo(6));
        }

        [Test]
        public void Update_ValidProduct_UpdatesProduct()
        {
            var productToUpdate = _unitOfWork.Product.Get(u => u.Id == 1);
            productToUpdate.Title = "Updated Product";
            _unitOfWork.Product.Update(productToUpdate);
            _unitOfWork.Save();

            var updatedProduct = _unitOfWork.Product.Get(u => u.Id == 1);
            Assert.That(updatedProduct.Title, Is.EqualTo("Updated Product"));
        }


    }


}
