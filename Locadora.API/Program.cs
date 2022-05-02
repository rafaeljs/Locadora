using Locadora.Domain.Interfaces.Repositories;
using Locadora.Domain.Interfaces.Services;
using Locadora.Repository.Context;
using Locadora.Repository.Repositories;
using Locadora.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Conexão banco MySQL
var connectionString = builder.Configuration["ConnectionStrings:Locadora"];
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
builder.Services.AddDbContext<LocadoraContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(connectionString, serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseLazyLoadingProxies()
        );
#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#region Injeção de dependencia
builder.Services.AddScoped<IFilmeService, FilmeService>();
builder.Services.AddScoped<ILocacaoService, LocacaoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<ILocacaoRepository, LocacaoRepository>();
builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<LocadoraContext>();
    dataContext.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
