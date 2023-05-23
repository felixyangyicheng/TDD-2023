using System;
using Moq;
using TDD.Contracts;
using TDD.Data;

namespace TDD.Test.Services
{

    public class LivreService
    {
        private readonly ILivreService _repository;

        public LivreService(ILivreService repository)
        {
            _repository = repository;
        }

        //public int Version
        //{
        //    get => _repository.Version;
        //    set => _repository.Version = value;
        //}

        public Task<List<Livre>> GetList() => _repository.GetAllBooks();

        public Task<bool> Update(Livre model) => _repository.Update(model.Isbn, model);

        public Task<Livre> GetBookByIsbn(string id) => _repository.GetBookByIsbn(id);

        public Task<bool> Delete(Livre model) => _repository.Delete(model.Isbn);
    }
    [TestClass]
    public class LivreServiceTest
	{
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }
        [TestMethod]



        public async Task<Livre> Get_1_Book()
        {
            var livreService = new Mock<ILivreService>();
            livreService.Setup(x => x.GetBookByIsbn("9090909090")).ReturnsAsync(new Livre ("9090909090","yes", "yes", "yes", Format.GrandFormat, true));
            return await livreService.Object.GetBookByIsbn("9090909090");
        }
        public void Create_Book_with_incomplet_info()
        {
            try{

            }
            catch
            {
                Assert.ThrowsException<NullReferenceException>(() => "livre avec informations incomplètes");
            }
        }
        [TestMethod]
        public void Update_Book_with_incomplet_info()
        {

            Assert.Fail();
        }
        [TestMethod]
        public void Delete_Book_with_reservation()
        {
            
            //Assert.ThrowsException<UnhandledExceptionEventArgs>(()=> )
        }
    }
}

