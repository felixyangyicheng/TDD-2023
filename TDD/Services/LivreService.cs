using System;
using TDD.Contracts;
using TDD.Data;

namespace TDD.Services
{
	public class LivreService:ILivreService
	{
       private ILivreService _livre;
        public LivreService(ILivreService livre)
        {
            this._livre = livre;
        }

        public Task<bool> Create(Livre livre)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<List<Livre>> GetAllBook()
        {
            throw new NotImplementedException();
        }

        public Task<List<Livre>> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public Task<List<Livre>> GetAvailableBooks()
        {
            throw new NotImplementedException();
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

        public Task<bool> Update(string isbn, Livre livre)
        {
            throw new NotImplementedException();
        }
    }
}

