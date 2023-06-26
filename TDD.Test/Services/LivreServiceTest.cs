using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TDD.Contracts;
using Microsoft.EntityFrameworkCore;

using TDD.Data;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;

namespace TDD.Test.Services
{

    [TestClass]
    public class LivreServiceTest
	{
        private  ILivreService _repository;
        private  BuDbContext _db;


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
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }

        [TestMethod]
        public async Task GetBooks_ReturnsAllBooks()
        {
            // Arrange
            var livres = new List<Livre>
            {
                new Livre { Isbn = "90909090909", Titre = "Book 1", Auteur = "Author 1", Editeur="Editeur 1", Format=Format.Broche, Disponible=true },
                new Livre { Isbn = "90909090901", Titre = "Book 2", Auteur = "Author 2", Editeur="Editeur 2", Format=Format.GrandFormat, Disponible=true },
                new Livre { Isbn = "90909090902", Titre = "Book 3", Auteur = "Author 3", Editeur="Editeur 3", Format=Format.Poche, Disponible=true },
       
            };
            _db.Livres.AddRange(livres);
            _db.SaveChanges();

            // Act
            var result = await _repository.GetAllBooks();

            // Assert
            Assert.AreEqual(livres.Count, result.Count());
            CollectionAssert.AreEqual(livres, result);
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
        public async Task CreateBook_AddsNewBook()
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
        public async Task UpdateBook_UpdatesExistingBook()
        {
            // Arrange
            var livreOrigine = new Livre { Isbn = "90909090909", Titre = "original Book 1", Auteur = "Author 2", Editeur = "Editeur 2", Format = Format.Broche, Disponible = true };
            _db.Livres.Add(livreOrigine);
            _db.SaveChanges();

            var livreMAJ = new Livre { Isbn = "90909090909", Titre = "update Book 1", Auteur = "Author 3", Editeur = "Editeur 3", Format = Format.Broche, Disponible = true };

            // Act
            var result = await _repository.Update("90909090909", livreMAJ);

            // Assert
            Assert.IsTrue(result);
            var livre = _db.Livres.FirstOrDefault(b => b.Isbn == livreOrigine.Isbn);
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
            _db.Livres.AddRange(livres);
            _db.SaveChanges();

            // Act
            var result = await _repository.GetAvailableBooks();

            // Assert
            Assert.AreEqual(livres.Count, result.Count());
            CollectionAssert.AreEqual(livres, result);
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
            _db.Livres.AddRange(livres);
            _db.SaveChanges();

            // Act
            var result = await _repository.GetBooksByAuthorName("Author 1");

            var livesAuthor1 = livres.Where(a => a.Auteur == "Author 1").ToList();
            // Assert
            Assert.AreEqual(livesAuthor1.Count, result.Count());
            CollectionAssert.AreEqual(livres, result);
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
            _db.Livres.AddRange(livres);
            _db.SaveChanges();

            // Act
            var result = await _repository.GetBooksByTitle("Book");

            var livesTitre = livres.Where(a => a.Titre.Contains("Book")).ToList();
            // Assert
            Assert.AreEqual(livesTitre.Count, result.Count());
            CollectionAssert.AreEqual(livres, result);
        }
    }
}

