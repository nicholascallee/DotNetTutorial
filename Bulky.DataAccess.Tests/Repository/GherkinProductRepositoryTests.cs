using BulkyBook.DataAccess.Repository.IRepository;
using Moq;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Newtonsoft.Json.Linq;
using BulkyBook.DataAccess.Repository;
using Microsoft.AspNetCore.Hosting;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [Binding]
    public class GherkinProductRepositoryTests
    {
        private static IUnitOfWork _unitOfWork;
        private static ApplicationDbContext _context;
        private static ServiceProvider _serviceProvider;
        private Product productToUpdate;
        private List<Product> results;
        private static Mock<IWebHostEnvironment> _mockEnvironment;
        private static ServiceCollection _serviceCollection;
        private static StartupTest _startupTest;

        private string updatedTitle = "Updated Product";


        [BeforeFeature]
        public static void Setup()
        {
            _serviceCollection = new ServiceCollection();

            // Create mock of IWebHostEnvironment
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockEnvironment.Setup(m => m.ContentRootPath).Returns("C:\\Development\\visualStudioProjects\\Bulky\\BulkyWeb");

            // Create startup instance
            _startupTest = new StartupTest(_mockEnvironment.Object);
            _startupTest.ConfigureServices(_serviceCollection);

            _serviceProvider = _serviceCollection.BuildServiceProvider();

            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Real TEst repository for integration tests
            _unitOfWork = new UnitOfWork(_context);
        }


        [Given(@"I have a product with ID (.*)")]
        public void GivenIHaveAProductWithId(int id)
        {
            productToUpdate = _unitOfWork.Product.Get(u => u.Id == id);
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

        [When(@"I update the (.*) of a product with value (.*)")]
        public void WhenIUpdateAColumnWithValue(string columnName, string value)
        {
            switch (columnName)
            {
                case ("Id"):
                    productToUpdate.Id = Int32.Parse(value);
                    break;

                case ("Title"):
                    productToUpdate.Title = value;
                    break;

                case ("Description"):
                    productToUpdate.Description = value;
                    break;

                case ("ISBN"):
                    productToUpdate.ISBN = value;
                    break;

                case ("Author"):
                    productToUpdate.Author = value;
                    break;

                case ("ListPrice"):
                    productToUpdate.ListPrice = Double.Parse(value);
                    break;

                case ("Price"):
                    productToUpdate.Price = Double.Parse(value);
                    break;

                case ("Price50"):
                    productToUpdate.Price50 = Double.Parse(value);
                    break;

                case ("Price100"):
                    productToUpdate.Price100 = Double.Parse(value);
                    break;

                default:
                    // Handle unexpected column names or throw an error
                    break;
            }

            _unitOfWork.Product.Update(productToUpdate);
            _unitOfWork.Save();
        }


        [Then(@"the product with (.*) should have its value updated with (.*)")]
        public void ThenTheProductWithColumnNameShouldHaveItsValueUpdated(string columnName, string value)
        {
            int intVal;
            Product updatedProduct;
            switch (columnName)
            {
                case ("Id"):
                    intVal = Int32.Parse(value);
                    updatedProduct = _unitOfWork.Product.Get(u => u.Id == intVal);
                    Assert.That(updatedProduct.Id, Is.EqualTo(intVal));
                    break;

                case ("Title"):
                    updatedProduct = _unitOfWork.Product.Get(u => u.Title == value);
                    Assert.That(updatedProduct.Title, Is.EqualTo(value));
                    break;

                case ("Description"):
                    updatedProduct = _unitOfWork.Product.Get(u => u.Description == value);
                    Assert.That(updatedProduct.Description, Is.EqualTo(updatedTitle));
                    break;

                case ("ISBN"):
                    updatedProduct = _unitOfWork.Product.Get(u => u.ISBN == value);
                    Assert.That(updatedProduct.ISBN, Is.EqualTo(updatedTitle));
                    break;

                case ("Author"):
                    updatedProduct = _unitOfWork.Product.Get(u => u.Author == value);
                    Assert.That(updatedProduct.Author, Is.EqualTo(updatedTitle));
                    break;

                case ("ListPrice"):
                    intVal = Int32.Parse(value);
                    updatedProduct = _unitOfWork.Product.Get(u => u.ListPrice == intVal);
                    Assert.That(updatedProduct.ListPrice, Is.EqualTo(intVal));
                    break;

                case ("Price"):
                    intVal = Int32.Parse(value);
                    updatedProduct = _unitOfWork.Product.Get(u => u.Price == intVal);
                    Assert.That(updatedProduct.Price, Is.EqualTo(intVal));
                    break;

                case ("Price50"):
                    intVal = Int32.Parse(value);
                    updatedProduct = _unitOfWork.Product.Get(u => u.Price50 == intVal);
                    Assert.That(updatedProduct.Price50, Is.EqualTo(intVal));
                    break;

                case ("Price100"):
                    intVal = Int32.Parse(value);
                    updatedProduct = _unitOfWork.Product.Get(u => u.Price100 == intVal);
                    Assert.That(updatedProduct.Price100, Is.EqualTo(intVal));
                    break;

                default:
                    // Handle unexpected column names or throw an error
                    break;
            }
            
        }

    }
}
