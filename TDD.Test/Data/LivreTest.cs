using System;
using TDD.Data;
namespace TDD.Test.DataTest
{
    [TestClass]
    public class LivreTest
	{
        [TestMethod]
        public void IsValidISBN_ValidISBN_ReturnsTrue()
        {
            // Arrange
            var livre = new Livre { Isbn = "9780123456789" };

            // Act
            var isValid = livre.IsValidISBN();

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidISBN_InvalidISBN_ReturnsFalse()
        {
            // Arrange
            var livre = new Livre { Isbn = "1234567890" };

            // Act
            var isValid = livre.IsValidISBN();

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}

