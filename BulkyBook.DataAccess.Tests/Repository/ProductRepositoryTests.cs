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
        private IProductRepository _productRepo;
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

            // Initialize and seed test database
            startup.Configure(_serviceProvider);

            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Instead of mocking, use real repository for integration tests
            _productRepo = new ProductRepository(_context);
        }

        [Test]
        public void GetAll_ReturnsAllProducts()
        {
            var results = _productRepo.GetAll().ToList();

            // Depending on how your DBInitializer seeds data, adjust the expected count
            Assert.That(results.Count, Is.EqualTo(6));
        }

        [Test]
        public void Update_UpdatesProduct()
        {
            var productToUpdate = _productRepo.Get(u => u.Id == 1);
            productToUpdate.Title = "Updated Product";
            _productRepo.Update(productToUpdate);

            var updatedProduct = _productRepo.Get(u => u.Id == 1);
            Assert.That(updatedProduct.Title, Is.EqualTo("Updated Product"));
        }

        [TearDown]
        public void Cleanup()
        {
            // Cleanup logic if needed (like clearing database or disposing resources)
            _context.Database.EnsureDeleted();  // Ensure DB is cleaned after each test
        }
    }


}
