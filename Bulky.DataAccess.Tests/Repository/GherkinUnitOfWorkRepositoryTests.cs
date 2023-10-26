using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Repository;
using Moq;
using TechTalk.SpecFlow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Linq.Expressions;
using System.Reflection;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [Binding]
    public class GherkinUnitOfWorkRepositoryTests
    {
        private static IUnitOfWork _unitOfWork;
        private static ApplicationDbContext _context;
        private static ServiceProvider _serviceProvider;
        private static Mock<IWebHostEnvironment> _mockEnvironment;
        private static ServiceCollection _serviceCollection;
        private static ApplicationRunner _testingApplication;
        dynamic repo;
        private Object inputClassColumnInstance;
        private object originalValue;
        private string changedColumnName;
        private string globalClass;
        private string globalColumn;
        private string globalOriginalValue;

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

        [Given(@"I have an instance of (.*) at (.*) with a known value of (.*)")]
        public void GivenIHaveAnInstanceOfBlankAtBlankWithAValueOfBlank(string className, string columnName, string value)
        {
            globalOriginalValue = value;
            // is the given class a valid property of UnitOfWork?
            var repoProperty = _unitOfWork.GetType().GetProperty(className);
            if (repoProperty == null)
                throw new InvalidOperationException($"No property named '{className}' found on unitOfWork");

            // grab the value of the property we just grabbed (_unitOfWork.ClassName)
            repo = repoProperty.GetValue(_unitOfWork);
            Type repoType = repo.GetType();
            Type entityType = repoType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>))
                ?.GetGenericArguments()[0];

            if (entityType == null)
            {
                throw new InvalidOperationException("Could not determine entity type from repository.");
            }

            // Construct the lambda
            var entityParam = Expression.Parameter(entityType, "entity");
            var propertyAccess = Expression.PropertyOrField(entityParam, columnName);
            var equalityCheck = Expression.Equal(propertyAccess, Expression.Constant(value, propertyAccess.Type));
            var lambdaType = typeof(Func<,>).MakeGenericType(entityType, typeof(bool));
            var lambda = Expression.Lambda(lambdaType, equalityCheck, entityParam);
            // Instead of searching directly on the repoType, we search on the IRepository<T> interface
            var iRepoType = typeof(Repository<>).MakeGenericType(entityType);
            MethodInfo getMethod = repo.GetType().GetMethod("Get", new[] { typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(entityType, typeof(bool))), typeof(string), typeof(bool) });
            if (getMethod == null)
            {
                throw new InvalidOperationException($"The Get method could not be found on the {iRepoType.Name} type.");
            }
            inputClassColumnInstance = getMethod.Invoke(repo, new object[] { lambda, null, false });
            if (inputClassColumnInstance == null)
            {
                throw new InvalidOperationException($"The repo has no item under the given filter. (repo.class.get() came back as null)");
            }

            PropertyInfo propertyInfo = inputClassColumnInstance.GetType().GetProperty(columnName);
            if (propertyInfo != null)
            {
                originalValue = propertyInfo.GetValue(inputClassColumnInstance);
                changedColumnName = columnName;
            }

        }


        [When(@"I update the instance of (.*) at (.*) with value (.*)")]
        public void WhenIUpdateTheBlankOfABlankWithValueBlank(string className, string columnName, string value)
        {
            globalClass = className;
            globalColumn = columnName;
            // Determine the entity type from the repository
            Type entityType = null;
            Type[] interfaces = repo.GetType().GetInterfaces();

            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IRepository<>))
                {
                    entityType = interfaceType.GetGenericArguments()[0];
                    break;
                }
            }

            if (entityType == null)
                throw new InvalidOperationException($"Could not determine the entity type for the repository.");

            // Use reflection to set the property value
            PropertyInfo propertyInfo = entityType.GetProperty(columnName);
            if (propertyInfo == null)
                throw new InvalidOperationException($"No property named '{columnName}' found on {entityType.Name}");

            // Convert the provided value to the appropriate type and set it
            object convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
            propertyInfo.SetValue(inputClassColumnInstance, convertedValue);
            string fullClassName = "BulkyBook.Models." + className + ", BulkyBook.Models";
            Type expectedType = Type.GetType(fullClassName);

            if (expectedType == null)
            {
                throw new InvalidOperationException($"Cannot find the type '{fullClassName}' in the currently loaded assemblies.");
            }

            if (inputClassColumnInstance.GetType() == expectedType)
            {
                MethodInfo updateMethod = repo.GetType().GetMethod("Update", new Type[] { expectedType });
                if (updateMethod == null)
                {
                    throw new InvalidOperationException($"The 'Update' method for type '{className}' could not be found on the repository.");
                }

                updateMethod.Invoke(repo, new object[] { inputClassColumnInstance });
            }
            else
            {
                throw new InvalidOperationException($"Expected inputClassColumnInstance to be of type '{className}', but got '{inputClassColumnInstance.GetType().Name}'.");
            }
            _unitOfWork.Save();
        }

        [Then(@"the instance of (.*) at (.*) should have its value updated as (.*)")]
        public void ThenTheInstanceOfBlankAtBlankShouldHaveItsValueUpdatedAsBlank(string className, string columnName, string value)
        {
            // Using reflection to get the actual value of the property
            PropertyInfo propertyInfo = inputClassColumnInstance.GetType().GetProperty(columnName);
            if (propertyInfo == null)
                throw new InvalidOperationException($"No property named '{columnName}' found on inputClassColumnInstance");

            // Re-query the unitOfWork for the given entity
            var updatedInstance = FetchInstanceFromUnitOfWork(className, columnName, value);

            // Ensure the updatedInstance is not null
            Assert.IsNotNull(updatedInstance, "The updated instance could not be retrieved from the repository.");

            // Ensure the value matches the expected value
            var actualValue = propertyInfo.GetValue(updatedInstance).ToString();
            Assert.That(actualValue, Is.EqualTo(value));
        }

        private object FetchInstanceFromUnitOfWork(string className, string columnName, string value)
        {

            // Get the repository from the unitOfWork based on the className
            var repoProperty = _unitOfWork.GetType().GetProperty(className);
            if (repoProperty == null)
                throw new InvalidOperationException($"No property named '{className}' found on unitOfWork");

            dynamic repoInstance = repoProperty.GetValue(_unitOfWork);


            Type repoType = repo.GetType();
            Type entityType = repoType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>))
                ?.GetGenericArguments()[0];

            if (entityType == null)
            {
                throw new InvalidOperationException("Could not determine entity type from repository.");
            }
            var entityParam = Expression.Parameter(entityType, "entity");
            var propertyAccess = Expression.PropertyOrField(entityParam, columnName);
            var equalityCheck = Expression.Equal(propertyAccess, Expression.Constant(Convert.ChangeType(value, propertyAccess.Type)));
            var lambdaType = typeof(Func<,>).MakeGenericType(entityType, typeof(bool));
            var lambda = Expression.Lambda(lambdaType, equalityCheck, entityParam);

            // Invoke the Get method on the repoInstance to fetch the entity
            MethodInfo getMethod = repoInstance.GetType().GetMethod("Get", new[] { typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(entityType, typeof(bool))), typeof(string), typeof(bool) });
            if (getMethod == null)
            {
                throw new InvalidOperationException($"The Get method could not be found on the repoInstance type.");
            }

            return getMethod.Invoke(repoInstance, new object[] { lambda, null, false });
        }


        [AfterScenario]
        public void ResetChangedValues()
        {
            WhenIUpdateTheBlankOfABlankWithValueBlank(globalClass, globalColumn, globalOriginalValue);
        }

    }
}
