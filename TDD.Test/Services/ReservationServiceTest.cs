using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TDD.Contracts;
using TDD.Data;
using TDD.Services;
using TDD.Test.Connections;

namespace TDD.Test.Services
{
    [TestClass]

    public class ReservationServiceTest
	{
        private IReservationService _reservation;
        private ILivreService _livres;
        private BuDbContext _db;
        private BuDbContext context;
        private ConnectionFactory factory;
        private ReservationService _reservationLocalRepo;
        private LivreService _livresLocalRepo;
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

             factory = new ConnectionFactory();
             context = factory.CreateContextForInMemory();
            _livresLocalRepo = new LivreService(context);
            _reservationLocalRepo = new ReservationService(context);
            context.Adherents.Add(user);
            context.SaveChanges();

            _db.Adherents.Add(user);
            _db.SaveChanges();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            _db.Database.EnsureDeleted();
            _db.Dispose();
        }

        #region CRUD


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
      
            context.Reservations.AddRange(reservations);
            context.SaveChanges();
            // Act
            var current = reservations.Where(r => r.DateFin > DateTime.Now.AddMonths(-4)).ToList();

            try
            {

            var result = await _reservationLocalRepo.GetCurrentReservationsByAdherentCode("UserCode");
            // Assert
            Assert.AreEqual(current.Count, result.Count());
            CollectionAssert.AreEqual(reservations, result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            var result = await _reservation.GetCurrentReservationsByAdherentCode("UserCode");

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
            var result = await _reservation.GetReservationById(1);
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
            var result = await _reservation.Create(reservation);

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
            var result = await _reservation.Update(1, reservationMAJ);

            // Assert
            Assert.IsTrue(result);
            var reservation = _db.Reservations.FirstOrDefault(b => b.Isbn == reservationOrigine.Isbn);
            Assert.IsNotNull(reservation);
            Assert.AreEqual(reservationMAJ.AdherentCode, reservation.AdherentCode);
            Assert.AreEqual(reservationMAJ.DateDebut, reservation.DateDebut);
            Assert.AreEqual(reservationMAJ.DateFin, reservation.DateFin);
        }
        [TestMethod]
        public async Task DeleteReservation_RemovesExistingReservation()
        {
            // Arrange
            var reservationOrigine = new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(2) };
            _db.Reservations.Add(reservationOrigine);
            _db.SaveChanges();
            // Act
            var result = await _reservation.Delete(1);
            // Assert
            Assert.IsTrue(result);
            var reservation = _db.Reservations.FirstOrDefault(b => b.Isbn == reservationOrigine.Isbn);
            Assert.IsNull(reservation);
        }
        #endregion

        #region BookAvailability
        [TestMethod]
        public async Task SetBookAvailabilityToFalse_OnReservationCreated()
        {
            // Arrange
            var reservation = new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now.AddDays(-30), DateFin = DateTime.Now.AddDays(30) };

            // Act
            var createReservation = await _reservation.Create(reservation);
            var livreResult = await _livres.GetBookByIsbn("90909090909");
            // Assert
            Assert.IsFalse(livreResult.Disponible);

        }
        #endregion

        #region SetEnd
        [TestMethod]
        public async Task SetEndOfReservation_ReturnTrue()
        {
            // Arrange
            var reservation = new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now.AddDays(-30), DateFin = DateTime.Now.AddDays(30) };
            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            // Act
            var result = await _reservation.SetEnd(1);
            // Assert
            Assert.IsTrue(result);

        }
        [TestMethod]
        public async Task SetEndOfReservation_EndDateValid()
        {
            // Arrange
            var reservation = new Reservation { Id = 1, Isbn = "90909090909", AdherentCode = "UserCode", DateDebut = DateTime.Now.AddDays(-30), DateFin = DateTime.Now.AddDays(30) };
            _db.Reservations.Add(reservation);
            _db.SaveChanges();
            // Act
            var result = await _reservation.SetEnd(1);
            // Assert
            var setEndDateReservation = await _reservation.GetReservationById(1);

            Assert.IsTrue(setEndDateReservation.DateFin < reservation.DateFin); 

        }
        #endregion
        #region Validity reservation

        [TestMethod]
        public void IsValidDuration_ValidDuration_4Months_ReturnsTrue()
        {
            // Arrange
            var reservation = new Reservation { Isbn = "9780123456789", AdherentCode= "UserCode",DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(2) };
            // Act
            var isValid = reservation.IsValidDuration4Months();
            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidDuration_InValidDuration_4Months_ReturnsFalse()
        {
            // Arrange
            var reservation = new Reservation { Isbn = "9780123456789", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddMonths(6) };
            // Act
            var isValid = reservation.IsValidDuration4Months();
            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsValidDuration_InValidDuration_EndDateBeforeStartDate_ReturnsFalse()
        {
            // Arrange
            var reservation = new Reservation { Isbn = "9780123456789", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(-1) };
            // Act
            var isValid = reservation.IsValidDuration4Months();
            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public async void IsValidReservationQuota_InValidQuota_MoreThan3_ReturnsFalse()
        {
            // Arrange
            var reservations = new List<Reservation>
            {
                new Reservation { Id=1,Isbn = "90909090909", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2) },
                new Reservation { Id=2,Isbn = "90909090901", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddDays(2)},
                new Reservation { Id=3,Isbn = "90909090902", AdherentCode="UserCode",DateDebut=DateTime.Now, DateFin=DateTime.Now.AddMonths(1)},
            };
            _db.Reservations.AddRange(reservations);
            _db.SaveChanges();

            var reservationOverflow = new Reservation { Isbn = "9780123456789", AdherentCode = "UserCode", DateDebut = DateTime.Now, DateFin = DateTime.Now.AddDays(2) };

            // Act
            var result = await _reservation.Create(reservationOverflow);
            // Assert
            Assert.IsFalse(result);
 
        }
        #endregion

    }
}

