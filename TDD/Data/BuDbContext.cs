using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TDD.Data
{
    public class MigrationHistory
    {
        public string MigrationId { get; set; }
        public string ProductVersion { get; set; }
    }
    public class BuDbContext:DbContext
	{
        public DbSet<Adherent> Adherents { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Livre> Livres { get; set; }
        public DbSet<MigrationHistory> __EFMigrationsHistory { get; set; }


        public BuDbContext(DbContextOptions<BuDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Primary Keys

            modelBuilder.Entity<MigrationHistory>().HasKey(mh => mh.MigrationId);
            #endregion Primary Keys

        }

    }
}

