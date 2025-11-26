using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Smart_Library_Management_System.Data;
using Smart_Library_Management_System.Mapping;
using Smart_Library_Management_System.Interfaces;
using Smart_Library_Management_System.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- services (DbContext, repos, AutoMapper, controllers) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- REPOSITORY DI REGISTRATION (REQUIRED) ---
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IFineRepository, FineRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();

// CORS
builder.Services.AddCors(o => o.AddPolicy("AllowLocalDev", p =>
    p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

// Serve index.html from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowLocalDev");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// SPA fallback
app.MapFallbackToFile("index.html");

app.Run();
