using Mario.Core.BusinessLayers;
using Mario.EF.Contexts;
using Mario.EF.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Configurazione del DbContext
builder.Services.AddDbContext<MarioDbContext>(options =>
{
    //options.UseSqlite("Data Source=mario.db;Mode=Memory;Cache=Shared");
    options.UseSqlite(builder.Configuration.GetConnectionString("InMemoryConnectionString"));
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
    //options.EnableSensitiveDataLogging(true);
});

// Repositories
builder.Services.AddTransient<CourseRepository>();
builder.Services.AddTransient<DishRepository>();

// Business Layers
builder.Services.AddTransient<MarioBusinessLayer>();

// Controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurazione di CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    });
});

// Build dell'applicazione
var app = builder.Build();

// Configurazione di SwaggerUI
app.UseSwagger();
app.UseSwaggerUI();

// Altre configurazioni
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();

// Mappatura dei controller
app.MapControllers();

// Avvio dell'applicazione
app.Run();
