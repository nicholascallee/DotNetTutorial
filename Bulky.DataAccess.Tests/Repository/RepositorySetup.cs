using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Tests.Repository
{
    public class RepositorySetup
    {

        public RepositorySetup() { 
            var testUnitOfWork = SetupUnitOfWork();
            pocExample(testUnitOfWork.Object);
        }



        public List<Category> setupCategories()
        {
            // Fake in-memory storage of Category objects
            List<Category> fakeCategoryStorage = new List<Category>
            {
                new Category { Id = 1, Name = "Initial Category", DisplayOrder = 2 }
                // ... other initial categories
            };
            return fakeCategoryStorage;
        }


        public Mock<ICategoryRepository> setupCategoryRepo(List<Category> fakeCategoryStorage)
        {
            // (SINGLE) Mock the repository
            var mockCategoryRepo = new Mock<ICategoryRepository>();

            // Setup Update method to modify the in-memory data
            mockCategoryRepo.Setup(repo => repo.Update(It.IsAny<Category>()))
                            .Callback<Category>(categoryToUpdate =>
                            {
                                var existingCategory = fakeCategoryStorage.FirstOrDefault(c => c.Id == categoryToUpdate.Id);
                                if (existingCategory != null)
                                {
                                    existingCategory.Name = categoryToUpdate.Name;
                                    existingCategory.DisplayOrder = categoryToUpdate.DisplayOrder;
                                }
                                else
                                {
                                    fakeCategoryStorage.Add(categoryToUpdate);
                                }
                            });
            return mockCategoryRepo;
        }


        public List<ApplicationUser> SetupApplicationUsers()
        {
            return new List<ApplicationUser>
            {
                // ... Your initial test ApplicationUser objects here
            };
        }

        public Mock<IApplicationUserRepository> SetupApplicationUserRepo(List<ApplicationUser> fakeApplicationUserStorage)
        {
            var mockRepo = new Mock<IApplicationUserRepository>();

            // You can set up methods for the mock repository as required.
            // Example:
            // mockRepo.Setup(repo => repo.Update(It.IsAny<ApplicationUser>()))
            //         .Callback<ApplicationUser>(...);

            return mockRepo;
        }

        public List<Company> SetupCompanies()
        {
            return new List<Company>
            {
                // ... Your initial test Company objects here
            };
        }

        public Mock<ICompanyRepository> SetupCompanyRepo(List<Company> fakeCompanyStorage)
        {
            var mockRepo = new Mock<ICompanyRepository>();

            // You can set up methods for the mock repository as required.
            // Example:
            // mockRepo.Setup(repo => repo.Update(It.IsAny<Company>()))
            //         .Callback<Company>(...);

            return mockRepo;
        }


        public List<OrderDetail> SetupOrderDetails()
        {
            return new List<OrderDetail>
            {
                // ... Your initial test OrderDetail objects here
            };
        }

        public Mock<IOrderDetailRepository> SetupOrderDetailRepo(List<OrderDetail> fakeOrderDetailStorage)
        {
            var mockRepo = new Mock<IOrderDetailRepository>();
            // Setup methods for the mock repository as required
            return mockRepo;
        }

        public List<OrderHeader> SetupOrderHeaders()
        {
            return new List<OrderHeader>
            {
                // ... Your initial test OrderHeader objects here
            };
        }

        public Mock<IOrderHeaderRepository> SetupOrderHeaderRepo(List<OrderHeader> fakeOrderHeaderStorage)
        {
            var mockRepo = new Mock<IOrderHeaderRepository>();
            // Setup methods for the mock repository as required
            return mockRepo;
        }

        public List<ProductImage> SetupProductImages()
        {
            return new List<ProductImage>
            {
                // ... Your initial test ProductImage objects here
            };
        }

        public Mock<IProductImageRepository> SetupProductImageRepo(List<ProductImage> fakeProductImageStorage)
        {
            var mockRepo = new Mock<IProductImageRepository>();
            // Setup methods for the mock repository as required
            return mockRepo;
        }

        public List<Product> SetupProducts()
        {
            return new List<Product>
            {
                // ... Your initial test Product objects here
            };
        }

        public Mock<IProductRepository> SetupProductRepo(List<Product> fakeProductStorage)
        {
            var mockRepo = new Mock<IProductRepository>();
            // Setup methods for the mock repository as required
            return mockRepo;
        }

        public List<ShoppingCart> SetupShoppingCarts()
        {
            return new List<ShoppingCart>
            {
                // ... Your initial test ShoppingCart objects here
            };
        }

        public Mock<IShoppingCartRepository> SetupShoppingCartRepo(List<ShoppingCart> fakeShoppingCartStorage)
        {
            var mockRepo = new Mock<IShoppingCartRepository>();
            // Setup methods for the mock repository as required
            return mockRepo;
        }


        public Mock<UnitOfWork> SetupUnitOfWork()
        {
            // Mock the ApplicationDbContext
            var mockContext = new Mock<ApplicationDbContext>();

            // Setup the mocked repositories using the methods provided before
            var fakeCategories = setupCategories();
            var mockCategoryRepo = setupCategoryRepo(fakeCategories);

            var fakeApplicationUsers = SetupApplicationUsers();
            var mockApplicationUserRepo = SetupApplicationUserRepo(fakeApplicationUsers);

            var fakeCompanies = SetupCompanies();
            var mockCompanyRepo = SetupCompanyRepo(fakeCompanies);

            var fakeOrderDetails = SetupOrderDetails();
            var mockOrderDetailRepo = SetupOrderDetailRepo(fakeOrderDetails);

            var fakeOrderHeaders = SetupOrderHeaders();
            var mockOrderHeaderRepo = SetupOrderHeaderRepo(fakeOrderHeaders);

            var fakeProductImages = SetupProductImages();
            var mockProductImageRepo = SetupProductImageRepo(fakeProductImages);

            var fakeProducts = SetupProducts();
            var mockProductRepo = SetupProductRepo(fakeProducts);

            var fakeShoppingCarts = SetupShoppingCarts();
            var mockShoppingCartRepo = SetupShoppingCartRepo(fakeShoppingCarts);

            // Mock the UnitOfWork
            var mockUnitOfWork = new Mock<UnitOfWork>();

            // Setup the properties of the UnitOfWork mock to return the mocked repositories
            mockUnitOfWork.Setup(uow => uow.Category).Returns(mockCategoryRepo.Object);
            mockUnitOfWork.Setup(uow => uow.ApplicationUser).Returns(mockApplicationUserRepo.Object);
            mockUnitOfWork.Setup(uow => uow.Company).Returns(mockCompanyRepo.Object);
            mockUnitOfWork.Setup(uow => uow.OrderDetail).Returns(mockOrderDetailRepo.Object);
            mockUnitOfWork.Setup(uow => uow.OrderHeader).Returns(mockOrderHeaderRepo.Object);
            mockUnitOfWork.Setup(uow => uow.ProductImage).Returns(mockProductImageRepo.Object);
            mockUnitOfWork.Setup(uow => uow.Product).Returns(mockProductRepo.Object);
            mockUnitOfWork.Setup(uow => uow.ShoppingCart).Returns(mockShoppingCartRepo.Object);

            return mockUnitOfWork;
        }


        public void pocExample(UnitOfWork mockUnitOfWork)
        {
            // Example of using the mock
            var before = mockUnitOfWork.Category.GetAll().Count();
            var testCategory = new Category { Id = 2, Name = "Updated Category", DisplayOrder = 5 };
            mockUnitOfWork.Category.Add(testCategory);
            var after = mockUnitOfWork.Category.GetAll().Count();
        }



    }
}
