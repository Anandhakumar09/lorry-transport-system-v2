using LorryTransport.Application.Interfaces;
using LorryTransport.Application.Services;
using LorryTransport.Infrastructure.Data;
using LorryTransport.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register the database connection (SQL Server via EF Core)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Register the generic repository for ALL entity types (Dependency Injection)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 3. Register our business logic services
builder.Services.AddScoped<ILoadEntryService, LoadEntryService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IDriverLedgerService, DriverLedgerService>();

// 4. Add Controllers + Swagger (API documentation/testing UI)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 5. Allow React (running on a different port) to call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        // Allow any localhost/127.0.0.1 port during local development.
        // Vite (or React dev server) can start on 5173, 5174, 5175... depending
        // on what's already running on your machine. Hardcoding just one port
        // here was causing "port different -> save fails" CORS errors whenever
        // the frontend happened to start on a different port.
        policy.SetIsOriginAllowed(origin =>
              {
                  if (Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                  {
                      return uri.Host == "localhost" || uri.Host == "127.0.0.1";
                  }
                  return false;
              })
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// NOTE: HTTPS redirection is intentionally left OFF for local development.
// We run the API on plain http://localhost:5000 so beginners don't need to
// trust a local dev HTTPS certificate. Enable UseHttpsRedirection() later
// when you deploy to a real server with a proper certificate.
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
