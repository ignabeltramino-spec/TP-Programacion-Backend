using Microsoft.EntityFrameworkCore;
using Tp_ProgramacionIII.Interfaces;
using Tp_ProgramacionIII.Models;
using Tp_ProgramacionIII.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<ITransactionService, TransactionService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirVue", app =>
    {
        app.WithOrigins("http://localhost:5173", "http://localhost:5174")
           .AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("PermitirVue");

app.UseAuthorization();

app.MapControllers();

app.Run();
