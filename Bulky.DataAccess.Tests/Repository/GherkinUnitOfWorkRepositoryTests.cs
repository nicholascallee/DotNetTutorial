using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Repository;
using Moq;
using TechTalk.SpecFlow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [Binding]
    public class GherkinUnitOfWorkRepositoryTests
    {
        private static IUnitOfWork _unitOfWork;
        private static ApplicationDbContext _context;
        private static ServiceProvider _serviceProvider;
        private Product productToUpdate;
        private static Mock<IWebHostEnvironment> _mockEnvironment;
        private static ServiceCollection _serviceCollection;
        private static ApplicationRunner _testingApplication;

        private Object inputClassColumnInstance;



        [BeforeFeature]
        public static void Setup()
        {
            _serviceCollection = new ServiceCollection();

            // Create mock of IWebHostEnvironment
            _mockEnvironment = new Mock<IWebHostEnvironment>();
            _mockEnvironment.Setup(m => m.ContentRootPath).Returns("C:\\Development\\visualStudioProjects\\Bulky\\BulkyWeb");

            // Create startup instance
            _testingApplication = new ApplicationRunner(_mockEnvironment.Object);
            _serviceCollection = (ServiceCollection)_testingApplication.ConfigureServices(_serviceCollection);

            _serviceProvider = _serviceCollection.BuildServiceProvider();

            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Real TEst repository for integration tests
            _unitOfWork = new UnitOfWork(_context);
        }


        [Given(@"I have an instance of (.*) at (.*) with a of value of (.*)")]
        public void GivenIHaveAnInstanceOfBlankAtBlankWithAValueOfBlank(string className, string columnName, string value)
        {
            //var inputClass = GetClass(className);
            //var inputClassColumn = GetClassColumn(inputClass);
            //inputClassColumnInstance = GetClassColumnInstance(inputClassColumn);
        }





        [When(@"I update the instance of (.*) at (.*) with value (.*)")]
        public void WhenIUpdateTheBlankOfABlankWithValueBlank(string className, string columnName, string value)
        {
            // Using reflection to get the repository from the UnitOfWork
            var repoProperty = _unitOfWork.GetType().GetProperty(className);
            if (repoProperty == null)
                throw new InvalidOperationException($"No property named '{className}' found on unitOfWork");

            dynamic repo = repoProperty.GetValue(_unitOfWork);

            // Assuming 'Get' is a method that accepts an expression, we're building an expression dynamically.
            var param = Expression.Parameter(Type.GetType(className), "u");
            var propertyAccess = Expression.PropertyOrField(param, columnName);
            var equalityCheck = Expression.Equal(propertyAccess, Expression.Constant(value, propertyAccess.Type));
            var lambda = Expression.Lambda(equalityCheck, param);

            inputClassColumnInstance = repo.Get(lambda);
        }

        [Then(@"the instance of (.*) at (.*) should have its value updated as (.*)")]
        public void ThenTheInstanceOfBlankAtBlankShouldHaveItsValueUpdatedAsBlank(string className, string columnName, string value)
        {
            var repoProperty = _unitOfWork.GetType().GetProperty(className);
            if (repoProperty == null)
                throw new InvalidOperationException($"No property named '{className}' found on unitOfWork");

            dynamic repo = repoProperty.GetValue(_unitOfWork);

            var param = Expression.Parameter(Type.GetType(className), "u");
            var propertyAccess = Expression.PropertyOrField(param, columnName);
            var equalityCheck = Expression.Equal(propertyAccess, Expression.Constant(value, propertyAccess.Type));
            var lambda = Expression.Lambda(equalityCheck, param);

            inputClassColumnInstance = repo.Get(lambda);
            var actualValue = Type.GetType(className).GetProperty(columnName).GetValue(inputClassColumnInstance, null);

            Assert.That(actualValue, Is.EqualTo(value));
        }
    }
}
