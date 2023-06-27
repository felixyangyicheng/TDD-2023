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
        //public LivreService(ILivreService livre)
        //{
        //    this._livre = livre;
        //}
        private  BuDbContext _db;

        public LivreService(
            BuDbContext db
            )
        {
            _db = db;
        }

        public async Task<bool> Create(Livre livre)
        {
            try
            {

            _db.Livres.Add(livre);
            }
            catch (Exception ex)
            {
                return false;
            }
            return await Save();
        }

        public async Task<bool> Delete(string isbn)
        {
            var livre=await _db.Livres.FirstOrDefaultAsync(a=>a.Isbn == isbn);
            if (livre != null)
            {

                _db.Livres.Remove(livre);
            }
            return await Save();
        }


        public async Task<List<Livre>> GetAllBooks()
        {
            return await _db.Livres.ToListAsync();
        }

        public async Task<List<Livre>> GetAvailableBooks()
        {
            return await _db.Livres.Where(a=>a.Disponible==true).ToListAsync();

        }

        public async Task<Livre> GetBookByIsbn(string isbn)
        {
            return await _db.Livres.FirstOrDefaultAsync(a => a.Isbn == isbn);
        }

        public async Task<List<Livre>> GetBooksByAuthorName(string authorName)
        {
            return await _db.Livres.Where(a => a.Auteur == authorName)
                .ToListAsync();

        }

        public async Task<List<Livre>> GetBooksByTitle(string name)
        {
            return await _db.Livres.Where(a => a.Titre.Contains( name))
            .ToListAsync();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(string isbn, Livre livre)
        {
            var rm = await _db.Livres.FirstOrDefaultAsync(a => a.Isbn == isbn);
            _db.Livres.Remove(rm);
            _db.Livres.Add(livre);


            return await Save();
        }
    }
}

