using System;
using TDD.Data;

namespace TDD.Contracts
{
	public interface ILivreService
	{
		public Task<bool> Create(Livre livre);
		public Task<bool> Update(string isbn,Livre livre);
		public Task<bool> Delete(string isbn);
		public Task<Livre> GetBookByIsbn(string isbn);
		public Task<List<Livre>> GetBooksByTitle(string name);
		public Task<List<Livre>> GetBooksByAuthorName(string authorName);
        public Task<List<Livre>> GetAllBooks();
        public Task<List<Livre>> GetAvailableBooks();
    }
}

