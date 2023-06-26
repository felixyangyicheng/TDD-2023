using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TDD.Data;
using TDD.Contracts;
using TDD.Services;

namespace TDD;
public class Program
{
    public static void Main(string[] args)
    {


        var builder = WebApplication.CreateBuilder(args);
        
        var connString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<BuDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString(connString)));
        builder.Services.AddScoped<ILivreService, LivreService>();
        builder.Services.AddScoped<IReservationService, ReservationService>();
        Console.WriteLine("yes");

        var app = builder.Build();
        app.Run();
    }
}

