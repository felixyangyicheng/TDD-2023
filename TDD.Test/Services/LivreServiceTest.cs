using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TDD.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Sqlite;

using Moq.EntityFrameworkCore;

using TDD.Data;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;
using TDD.Test.Connections;
using TDD.Services;
using System.Net.Sockets;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.EntityFrameworkCore.Query;
using static TDD.Test.Services.LivreServiceTest;
using System.Linq.Expressions;
using TDD.Test.Helpers;

namespace TDD.Test.Services
{

    [TestClass]
    public class LivreServiceTest
	{
        private  ILivreService _repository;
        private  BuDbContext _db;

        private  Mock<BuDbContext> _mockContext;
        private  LivreService _livreService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Utilisation de SQLite en mémoire pour les tests
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<BuDbContext>()
                .UseSqlite(connection)
                .Options;

            _db = new BuDbContext(options);
            _db.Database.EnsureCreated();


            _repository = new LivreService(_db);

        }


        [TestMethod]
        public async Task GetBooks_ReturnsAllBooks()
        {
            try
            {
                //Arrange    
                var factory = new ConnectionFactory();
                var context = factory.CreateContextForInMemory();

                var livres = new List<Livre>
                {
                    new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                    new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 2", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=true },
                    new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },
       
                };
                context.Livres.AddRange(livres);
                context.SaveChanges();

                // Act
                var localRepo = new LivreService(context);
                var result = await localRepo.GetAllBooks();

                // Assert
                Assert.AreEqual(livres.Count, result.Count());
                //CollectionAssert.AreEqual(livres, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw (ex);
            }
        }



        [TestMethod]
        public async Task GetAllBooks_ReturnsAllBooks_Mock()
        {
            //arrange
            _mockContext = new Mock<BuDbContext>();
            var livres = new List<Livre>
                {
                    new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                    new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 2", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=true },
                    new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },
                }.AsQueryable();
            var mockSet = new Mock<DbSet<Livre>>();
            mockSet.As<IAsyncEnumerable<Livre>>()
                    .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                    .Returns(new TestAsyncEnumerator<Livre>(livres.AsEnumerable().GetEnumerator()));
            mockSet.As<IQueryable<Livre>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Livre>(livres.AsQueryable().Provider));
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Provider).Returns(livres.AsQueryable().Provider);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Expression).Returns(livres.AsQueryable().Expression);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.ElementType).Returns(livres.AsQueryable().ElementType);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.GetEnumerator()).Returns(livres.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Livres).Returns(mockSet.Object);
            _livreService = new LivreService(_mockContext.Object);
            //Act
            var result = await _livreService.GetAllBooks();
                // Assert
                Assert.IsNotNull(result);
                CollectionAssert.AreEqual(result, livres.ToList());
  


        }
        [TestMethod]
        public async Task GetBook_ReturnsBookByIsbn()
        {
            // Arrange
            var livre = new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur = "Editeur 1", Format = Format.Broche, Disponible = true };

            _db.Livres.Add(livre);
            _db.SaveChanges();

            // Act
            var result = await _repository.GetBookByIsbn("90909090909");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(livre.Isbn, result.Isbn);
        }
        [TestMethod]
        public async Task GetBook_ReturnsBookByIsbn_Mock()
        {
            //arrange
            _mockContext = new Mock<BuDbContext>();
            var livres = new List<Livre>
                {
                    new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                    new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 2", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=true },
                    new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },
                }.AsQueryable();
            var mockSet = new Mock<DbSet<Livre>>();
            mockSet.As<IAsyncEnumerable<Livre>>()
                    .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                    .Returns(new TestAsyncEnumerator<Livre>(livres.AsEnumerable().GetEnumerator()));
            mockSet.As<IQueryable<Livre>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Livre>(livres.AsQueryable().Provider));
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Provider).Returns(livres.AsQueryable().Provider);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Expression).Returns(livres.AsQueryable().Expression);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.ElementType).Returns(livres.AsQueryable().ElementType);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.GetEnumerator()).Returns(livres.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Livres).Returns(mockSet.Object);
            _livreService = new LivreService(_mockContext.Object);
            //Act
            try
            {

            var result = await _livreService.GetBookByIsbn("90909090909");
            Assert.AreEqual(result, livres.FirstOrDefault(a=>a.Isbn== "90909090909"));
            // Assert
            Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [TestMethod]
        public async Task CreateBook_AddsNewBookWithCompleteInfo()
        {
            // Arrange
            var livre = new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur = "Editeur 1", Format = Format.Broche, Disponible = true };

            // Act
            var result = await _repository.Create(livre);

            // Assert
            var livreCree = _db.Livres.FirstOrDefault(b => b.Isbn == livre.Isbn);
            Assert.IsNotNull(livreCree);
            Assert.IsTrue(result);
            Assert.AreEqual(livre, livreCree);
        }

        [TestMethod]
        public async Task CreateBook_InvalidBook_DoesNotAddBookToDatabase()
        {
            // Arrange
            var livre = new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1"};

    
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForInMemory();
            var localRepo = new LivreService(context);
      
            var result = await localRepo.Create(livre);

            // Assert
            var livreCree = _db.Livres.FirstOrDefault(b => b.Isbn == livre.Isbn);
            Assert.IsNull(livreCree);
            Assert.Fail();
            Assert.IsFalse(result);

        }
        [TestMethod]
        public async Task UpdateBook_UpdatesExistingBook()
        {
            // Arrange
            var livreOrigine = new Livre { Isbn = "90909090909", Titre = "original Book 1", Auteur = "Author 2", Editeur = "Editeur 2", Format = Format.Broche, Disponible = true };
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForInMemory();
            var localRepo = new LivreService(context);

            context.Livres.Add(livreOrigine);
            context.SaveChanges();

            var livreMAJ = new Livre { Isbn = "90909090909", Titre = "update Book 1", Auteur = "Author 3", Editeur = "Editeur 3", Format = Format.Broche, Disponible = true };

            // Act
            var result = await localRepo.Update("90909090909", livreMAJ);

            // Assert

            var livre = context.Livres.FirstOrDefault(b => b.Isbn == livreOrigine.Isbn);
            Assert.IsNotNull(livre);
            Assert.IsTrue(result);
            Assert.AreEqual(livreMAJ.Titre, livre.Titre);
            Assert.AreEqual(livreMAJ.Auteur, livre.Auteur);
            Assert.AreEqual(livreMAJ.Editeur, livre.Editeur);
        }
        [TestMethod]
        public async Task DeleteBook_RemovesExistingBook()
        {
            // Arrange
            var livreOrigine = new Livre { Isbn = "90909090909", Titre = "update Book 1", Auteur = "Author 3", Editeur = "Editeur 3", Format = Format.Broche, Disponible = true };
            _db.Livres.Add(livreOrigine);
            _db.SaveChanges();

            // Act
            var result = await _repository.Delete("90909090909");

            // Assert
            Assert.IsTrue(result);
            var livre = _db.Livres.FirstOrDefault(b => b.Isbn == livreOrigine.Isbn);
            Assert.IsNull(livre);
        }

        [TestMethod]
        public async Task GetBooks_ReturnsAvailableBooks()
        {
            // Arrange
            var livres = new List<Livre>
            {
                new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 2", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=false },
                new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },

            };
            var factory = new ConnectionFactory();

            //Get the instance of BlogDBContext  
            var context = factory.CreateContextForInMemory();
            var localRepo = new LivreService(context);
            context.Livres.AddRange(livres);
            context.SaveChanges();

            // Act
            var result = await localRepo.GetAvailableBooks();

            // Assert
            Assert.AreEqual(livres.Where(a=>a.Disponible==true).ToList().Count, result.Count());
            CollectionAssert.AreEqual(livres.Where(a => a.Disponible == true).ToList(), result);
        }



        [TestMethod]
        public async Task GetBooks_ReturnsAvailableBooks_Mock()
        {
            // Arrange
            _mockContext = new Mock<BuDbContext>();
            var livres = new List<Livre>
                {
                    new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                    new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 2", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=true },
                    new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },
                }.AsQueryable();
            var mockSet = new Mock<DbSet<Livre>>();
            mockSet.As<IAsyncEnumerable<Livre>>()
                    .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                    .Returns(new TestAsyncEnumerator<Livre>(livres.AsEnumerable().GetEnumerator()));
            mockSet.As<IQueryable<Livre>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Livre>(livres.AsQueryable().Provider));
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Provider).Returns(livres.AsQueryable().Provider);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.Expression).Returns(livres.AsQueryable().Expression);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.ElementType).Returns(livres.AsQueryable().ElementType);
            mockSet.As<IQueryable<Livre>>().Setup(m => m.GetEnumerator()).Returns(livres.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Livres).Returns(mockSet.Object);
            _livreService = new LivreService(_mockContext.Object);

            // Act
      
                var result = await _livreService.GetAvailableBooks();

                // Assert
                Assert.AreEqual(livres.Where(a => a.Disponible == true).ToList().Count, result.Count());
                CollectionAssert.AreEqual(livres.ToList(), result);
   
    
        }
        [TestMethod]
        public async Task GetBooks_ReturnsBooksByAuthorName()
        {
            // Arrange
            var livres = new List<Livre>
            {
                new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 1", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=false },
                new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },

            };
            var factory = new ConnectionFactory();

            //Get the instance of BlogDBContext  
            var context = factory.CreateContextForInMemory();
            var localRepo = new LivreService(context);
            context.Livres.AddRange(livres);
            context.SaveChanges();

            // Act
            var result = await localRepo.GetBooksByAuthorName("Author 1");

            var livesAuthor1 = livres.Where(a => a.Auteur == "Author 1").ToList();
            // Assert
            Assert.AreEqual(livesAuthor1.Count, result.Count());
            CollectionAssert.AreEqual(livesAuthor1, result);
        }

        [TestMethod]
        public async Task GetBooks_ReturnsBooksByTitle()
        {
            // Arrange
            var livres = new List<Livre>
            {
                new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 1", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=false },
                new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },

            };

            var factory = new ConnectionFactory();

            //Get the instance of BlogDBContext  
            var context = factory.CreateContextForInMemory();
            var localRepo = new LivreService(context);
            context.Livres.AddRange(livres);
            context.SaveChanges();

            // Act
            var result = await localRepo.GetBooksByTitle("Book");

            var livesTitre = livres.Where(a => a.Titre.Contains("Book")).ToList();
            // Assert
            Assert.AreEqual(livesTitre.Count, result.Count());
            CollectionAssert.AreEqual(livesTitre, result);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }





    }
}

