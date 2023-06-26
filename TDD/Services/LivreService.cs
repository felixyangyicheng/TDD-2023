using System;
using Microsoft.EntityFrameworkCore;
using TDD.Contracts;
using TDD.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TDD.Services
{
	public class LivreService:ILivreService
	{
       //private ILivreService _livre;
       // public LivreService(ILivreService livre)
       // {
       //     this._livre = livre;
       // }
        private readonly BuDbContext _db;

        public LivreService(
            BuDbContext db
            )
        {
            _db = db;
        }

        public async Task<bool> Create(Livre livre)
        {
            _db.Livres.Add(livre);
            return await Save();
        }

        public Task<bool> Delete(string isbn)
        {
            throw new NotImplementedException();
        }


        public async Task<List<Livre>> GetAllBooks()
        {
            return await _db.Livres.ToListAsync();
        }

        public async Task<List<Livre>> GetAvailableBooks()
        {
            return await _db.Livres.Where(a=>a.Disponible==true).ToListAsync();

        }

        public Task<Livre> GetBookByIsbn(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<List<Livre>> GetBooksByAuthorName(string authorName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Livre>> GetBooksByTitle(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public Task<bool> Update(string isbn, Livre livre)
        {
            throw new NotImplementedException();
        }
    }
}

