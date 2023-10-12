using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Tests.Repository
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private ProductRepository _repository;

        [TestInitialize]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _repository = new ProductRepository(_mockContext.Object);
            // Further setup for mocking DbSet etc.
        }

        [TestMethod]
        public void Add_ValidProduct_IncreasesCount()
        {
            // Arrange
            //var product = new Product { /* initialization */ };

            // Act
            //_repository.Add(product);

            // Assert
            // assert that the mock context's products now include the added product.
            
        }

        // Additional tests for other methods...
    }

}
