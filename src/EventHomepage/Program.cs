using EventInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlite("Data Source=eventdatabase.db"));

var app = builder.Build();

app.MapStaticAssets();
app.MapDefaultControllerRoute();

app.Run();
