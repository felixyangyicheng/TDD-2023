using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TDD.Contracts;
using TDD.Data;

namespace TDD.Test.Services
{
	public class ReservationServiceTest
	{
        private IReservationService _repository;
        private BuDbContext _db;


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
            var user = new Adherent
            {
                Code = "UserCode",
                Nom = "User",
                Prenom = "User",
                Civilite = "Monsieur",
                DateNaissance = DateTime.Now.AddYears(-18),
            };
            _db.Adherents.Add(user);
            _db.SaveChanges();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }
        [TestMethod]
        public async Task GetReservation_ReturnsUserCurrentReservations()
        {
            // Arrange

            var reservations = new List<Reservation>
            {
                new Reservation { Id=1,Isbn = "90909090909", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2) },
                new Reservation { Id=2,Isbn = "90909090901", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2)},
                new Reservation { Id=3,Isbn = "90909090902", AdherentCode="UserCode",DateDebut=DateTime.Now.AddMonths(-8), DateFin=DateTime.Now.AddMonths(-6)},
                new Reservation { Id=4,Isbn = "90909090903", AdherentCode="UserCode",DateDebut=DateTime.Now.AddMonths(-8), DateFin=DateTime.Now.AddMonths(-6)},

            };
      
            _db.Reservations.AddRange(reservations);
            _db.SaveChanges();
            // Act
            var current = reservations.Where(r => r.DateFin > DateTime.Now.AddMonths(-4)).ToList();
            var result = await _repository.GetCurrentReservationsByAdherentCode("UserCode");
            // Assert
            Assert.AreEqual(current.Count, result.Count());
            CollectionAssert.AreEqual(reservations, result);
        }
        [TestMethod]
        public async Task GetReservation_ReturnsUserHistoricalReservations()
        {
            // Arrange

            var reservations = new List<Reservation>
            {
                new Reservation { Id=1,Isbn = "90909090909", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2) },
                new Reservation { Id=2,Isbn = "90909090901", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2)},
                new Reservation { Id=3,Isbn = "90909090902", AdherentCode="UserCode",DateDebut=DateTime.Now.AddMonths(-8), DateFin=DateTime.Now.AddMonths(-6)},
                new Reservation { Id=4,Isbn = "90909090903", AdherentCode="UserCode",DateDebut=DateTime.Now.AddMonths(-8), DateFin=DateTime.Now.AddMonths(-6)},
            };
            _db.Reservations.AddRange(reservations);
            _db.SaveChanges();
            // Act
            var result = await _repository.GetCurrentReservationsByAdherentCode("UserCode");

            // Assert
            Assert.AreEqual(reservations.Count, result.Count());
            CollectionAssert.AreEqual(reservations, result);
        }

        [TestMethod]
        public async Task GetReservation_ReturnsReservationById()
        {
            // Arrange
            var reservations = new List<Reservation>
            {
                new Reservation { Id=1,Isbn = "90909090909", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2) },
                new Reservation { Id=2,Isbn = "90909090901", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2)},
                new Reservation { Id=3,Isbn = "90909090902", AdherentCode="UserCode",DateDebut=DateTime.Now.AddMonths(-8), DateFin=DateTime.Now.AddMonths(-6)},
                new Reservation { Id=4,Isbn = "90909090903", AdherentCode="UserCode",DateDebut=DateTime.Now.AddMonths(-8), DateFin=DateTime.Now.AddMonths(-6)},

            };
            _db.Reservations.AddRange(reservations);
            _db.SaveChanges();
            // Act
            var result = await _repository.GetReservationById(1);
            var reservation = reservations.FirstOrDefault(a => a.Id == 1);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reservation.Isbn, result.Isbn);
        }
        [TestMethod]
        public async Task CreateReservation_AddsNewReservation()
        {
            // Arrange
            var reservation = new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(2) };

            // Act
            var result = await _repository.Create(reservation);

            // Assert
            var newReservation = _db.Reservations.FirstOrDefault(b => b.Isbn == reservation.Isbn);
            Assert.IsNotNull(newReservation);
            Assert.IsTrue(result);
            Assert.AreEqual(reservation, newReservation);
        }
        [TestMethod]
        public async Task UpdateReservation_UpdatesExistingReservation()
        {
            // Arrange
            var reservationOrigine =   new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(2) };
            _db.Reservations.Add(reservationOrigine);
            _db.SaveChanges();

            var reservationMAJ = new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(3) };

            // Act
            var result = await _repository.Update(1, reservationMAJ);

            // Assert
            Assert.IsTrue(result);
            var reservation = _db.Reservations.FirstOrDefault(b => b.Isbn == reservationOrigine.Isbn);
            Assert.IsNotNull(reservation);
            Assert.AreEqual(reservationMAJ.AdherentCode, reservation.AdherentCode);
            Assert.AreEqual(reservationMAJ.DateDebut, reservation.DateDebut);
            Assert.AreEqual(reservationMAJ.DateFin, reservation.DateFin);
        }
        [TestMethod]
        public async Task DeleteBook_RemovesExistingBook()
        {
            // Arrange
            var reservationOrigine = new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(2) };
            _db.Reservations.Add(reservationOrigine);
            _db.SaveChanges();
            // Act
            var result = await _repository.Delete(1);
            // Assert
            Assert.IsTrue(result);
            var reservation = _db.Reservations.FirstOrDefault(b => b.Isbn == reservationOrigine.Isbn);
            Assert.IsNull(reservation);
        }

    }
}

