using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Tests.Data
{
    [TestFixture]
    public class ApplicationDbContextTests
    {
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            _context = MockDbGenerator.Create();
        }

        [TearDown]
        public void TearDown()
        {
            MockDbGenerator.Destroy(_context);
        }

        [Test]
        public void SeededCategories_ShouldBeCorrect()
        {
            // Arrange
            // ... Any necessary arrangement for this test.
            // Sample list of category names to check for
            List<string> categoryNamesToCheck = new List<string>
            {
                "Action",
                "Sci-Fi",
                "Comedy",
                "History"
            };

            // Act: EF Core would run OnModelCreating and seed data here
            var categories = _context.Categories.ToListAsync().Result;

            // Assert
            Assert.AreEqual(4, categories.Count);
            foreach (string categoryName in categoryNamesToCheck)
            {
                Assert.IsTrue(categories.Exists(c => c.Name == categoryName), $"Category '{categoryName}' not found.");
            }
            // ... Add more assertions for the other categories and their properties.

        }

        [Test]
        public void SeededCompanies_ShouldBeCorrect()
        {
            // Arrange
            var expectedCompanies = new List<Company>
    {
        new Company
        {
            Id = 1,
            Name = "Tech Solution",
            StreetAddress = "123 Tech St",
            City = "Tech City",
            PostalCode = "12121",
            State = "IL",
            PhoneNumber = "6669990000"
        },
        new Company
        {
            Id = 2,
            Name = "Vivid Books",
            StreetAddress = "999 Vid St",
            City = "Vid City",
            PostalCode = "66666",
            State = "IL",
            PhoneNumber = "7779990000"
        },
        new Company
        {
            Id = 3,
            Name = "Readers Club",
            StreetAddress = "999 Main St",
            City = "Lala land",
            PostalCode = "99999",
            State = "NY",
            PhoneNumber = "1113335555"
        }
    };

            // Act
            var companies = _context.Companies.ToListAsync().Result;

            // Assert
            Assert.AreEqual(expectedCompanies.Count, companies.Count);
            foreach (var expectedCompany in expectedCompanies)
            {
                Assert.IsTrue(companies.Exists(c =>
                    c.Id == expectedCompany.Id &&
                    c.Name == expectedCompany.Name &&
                    c.StreetAddress == expectedCompany.StreetAddress &&
                    c.City == expectedCompany.City &&
                    c.PostalCode == expectedCompany.PostalCode &&
                    c.State == expectedCompany.State &&
                    c.PhoneNumber == expectedCompany.PhoneNumber
                ));
            }
        }

        [Test]
        public void SeededProducts_ShouldBeCorrect()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    ISBN = "SWD9999001",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    ISBN = "CAW777777701",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    ISBN = "RITO5555501",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    ISBN = "WS3333333301",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    ISBN = "SOTJ1111111101",
                    CategoryId = 3
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    ISBN = "FOT000000001",
                    CategoryId = 2
                }
            };


            // Act
            var products = _context.Products.ToListAsync().Result;

            // Assert
            Assert.AreEqual(expectedProducts.Count, products.Count);
            foreach (var expectedProduct in expectedProducts)
            {
                Assert.IsTrue(products.Exists(p =>
                    p.Id == expectedProduct.Id &&
                    p.Title == expectedProduct.Title &&
                    p.Author == expectedProduct.Author &&
                    p.ISBN == expectedProduct.ISBN &&
                    p.CategoryId == expectedProduct.CategoryId
                ));
            }
        }




        // ... other tests
    }

}
