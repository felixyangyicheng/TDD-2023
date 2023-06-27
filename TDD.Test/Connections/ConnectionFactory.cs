using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TDD.Data;

namespace TDD.Test.Connections
{
	public class ConnectionFactory : IDisposable
    {
        #region IDisposable Support  
        private bool disposedValue = false; // To detect redundant calls  

        public BuDbContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<BuDbContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;

            var context = new BuDbContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        public BuDbContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<BuDbContext>()
                .UseSqlite(connection)
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)

                .Options;

            var context = new BuDbContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}

