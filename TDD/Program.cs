using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TDD.Data;

namespace TDD;
public class Program
{
    public static void Main(string[] args)
    {


        var builder = WebApplication.CreateBuilder(args);
        
        var connString = builder.Configuration.GetConnectionString("sqlite");
        builder.Services.AddDbContextPool<BuDbContext>(options =>
        {
            options.UseSqlite(connString);
        });

        Console.WriteLine("yes");

                   var app = builder.Build();
        app.Run();
    }
}

