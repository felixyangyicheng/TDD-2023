using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using TDD.Data;
using TDD.Services;

namespace TDD.Test.Services
{
    [TestClass]

    public class LivreServiceMockTest
	{


        List<Livre> entities;
        [TestMethod]
        public async Task GetAllBooks_ReturnsAllBooks_Mock()
        {
            // Arrange

            var expectedBooks = new List<Livre>
            {
                new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 2", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=true },
                new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },

            }.AsQueryable();
            var mockSet = new Mock<DbSet<Livre>>();
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Provider).Returns(expectedBooks.Provider);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Expression).Returns(expectedBooks.Expression);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.ElementType).Returns(expectedBooks.ElementType);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.GetEnumerator()).Returns(() => expectedBooks.GetEnumerator());


            var mockContext = new Mock<BuDbContext>();
            mockContext.Setup(c => c.Livres).Returns(mockSet.Object);

            var service = new LivreService(mockContext.Object);

            // Act

            var result = service.GetAllBooks();
            var actualBooks = result as IEnumerable<Livre>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(actualBooks);
            Assert.AreEqual(expectedBooks.ToList().Count, actualBooks.Count());
        }
    }
}

