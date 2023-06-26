using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TDD.Contracts;
using Microsoft.EntityFrameworkCore;

using TDD.Data;
using System.Text;

namespace TDD.Test.Services
{

    public class LivreService
    {
        private readonly ILivreService _repository;
        private readonly BuDbContext _db;

        public LivreService(ILivreService repository)
        {
            _repository = repository;
        }
        public LivreService(BuDbContext db)
        {
            _db = db;
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
        //static IWebHost _webHost = null;
        //static T GetService<T>()
        //{
        //    var scope = _webHost.Services.CreateScope();
        //    return scope.ServiceProvider.GetRequiredService<T>();
        //}

        //[ClassInitialize]
        //public static void Init(TestContext testContext)
        //{
        //    _webHost = WebHost.CreateDefaultBuilder()
        //        .ConfigureServices(a =>
        //    {
        //        a.AddDbContextPool<BuDbContext>(options =>
        //        {
        //            options.UseSqlite("");
        //        });
        //    }).Build();
        //}
        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {
        }
        [TestMethod]



        //public async Task<Livre> Get_1_Book()
        //{
        //    var livreService = new Mock<ILivreService>();
        //    livreService.Setup(x => x.GetBookByIsbn("9090909090")).ReturnsAsync(new Livre ("9090909090","yes", "yes", "yes", Format.GrandFormat, true));
        //    return await livreService.Object.GetBookByIsbn("9090909090");
        //}

        public void Insert()
        {

            var dbPath = "test.sqlite";
            if (File.Exists(dbPath)) File.Delete(dbPath);

            var toDb = new Livre
            {
         Isbn="9090909090",
                Auteur="yes",
                Editeur="yes",
                Titre="yes", Format=Format.GrandFormat, Disponible=true
            };
     
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


        [TestMethod]
        public void TestMethod1()
        {
            var dbPath = "test.sqlite";
            if (File.Exists(dbPath)) File.Delete(dbPath);
            var opt = new DbContextOptionsBuilder<BuDbContext>()
                .UseSqlite($"data source={dbPath}").Options;
            var ctx = new BuDbContext(opt);

            Assert.IsTrue(ctx.Database.EnsureCreated());
            ctx.Database.EnsureCreated();
            var resp = new LivreService(ctx);
            Assert.AreEqual(ctx.Livres.Count(), 0);
          
        }
    }
}

