using BulkyBook.DataAccess.Repository.IRepository;
using Moq;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [Binding]
    public class GherkinProductRepositoryTests
    {
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _context;
        private ServiceProvider _serviceProvider;
        private Product productToUpdate;
        private string updatedTitle = "Updated Product";
        private List<Product> results;
        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Add any necessary services for your application
            // For example: services.AddDbContext<ApplicationDbContext>(...);

            _serviceProvider = services.BuildServiceProvider();

            _context = _serviceProvider.GetService<ApplicationDbContext>();
            _unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
        }

        [When(@"I retrieve all products")]
        public void WhenIRetrieveAllProducts()
        {
            results = _unitOfWork.Product.GetAll().ToList();
        }

        [Then(@"the number of products should be (.*)")]
        public void ThenTheNumberOfProductsShouldBe(int expectedCount)
        {
            Assert.That(results.Count, Is.EqualTo(expectedCount));
        }

        [When(@"I update a product with ID (.*)")]
        public void WhenIUpdateAProductWithID(int productId)
        {
            productToUpdate = _unitOfWork.Product.Get(u => u.Id == productId);
            productToUpdate.Title = updatedTitle;
            _unitOfWork.Product.Update(productToUpdate);
            _unitOfWork.Save();
        }

        [Then(@"the product with ID (.*) should have its title updated")]
        public void ThenTheProductWithIDShouldHaveItsTitleUpdated(int productId)
        {
            var updatedProduct = _unitOfWork.Product.Get(u => u.Id == productId);
            Assert.That(updatedProduct.Title, Is.EqualTo(updatedTitle));
        }
    }
}
