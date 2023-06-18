using Sudoku.Scraper.API.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureApplicationHost();

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHostedServices();
builder.Services.ConfigureApplicationOptions(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddDatabases(builder.Configuration);

var app = builder.Build();

await app.MigrateDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();