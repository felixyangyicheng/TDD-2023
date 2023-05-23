using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TDD.Data
{
    public class BuDbContext:DbContext
	{
        public DbSet<Adherent> Adherents { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Livre> Livres { get; set; }


        public BuDbContext(DbContextOptions<BuDbContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=BuDatabase.db");
        }


    }
}

