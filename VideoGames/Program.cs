using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using Serilog;
using VideoGames.BLL.Services;
using VideoGames.BLL.Tools;
using VideoGames.BLL.Validators.Game;
using VideoGames.DAL;
using VideoGames.DAL.Initializer;
using VideoGames.DAL.Repositories;
using VideoGames.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console() 
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("LocalDb");
    if (connectionString == null)
    {
        throw new Exception("Connection string 'LocalDb' not found in configuration.");
    }
    options.UseNpgsql(connectionString);
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODE3OTQyNDAwIiwiaWF0IjoiMTc4NjQ0NjkxOCIsImFjY291bnRfaWQiOiIwMTk5NTEzZTdlYmY3YjYwOGI4Y2I3NTI3YTE3ZTI5MyIsImN1c3RvbWVyX2lkIjoiMDE5OTUxM2U3ZWJmN2I20mhkY2I3NTI3YTE3ZTI5MyIsInN1Yl9pZCI6Ii0iLCJlZGl0aW9uIjoiMCIsInR5cGUiOiIyIn0.gnQYP7aLCcVQ_aS_g36BR2TVz1srfcCr3P5xrAw-1S6MNPECaqNweRUZCwbe6OKG6QL64wtDIYoFmuchoaQSmtAXDRldrVvsOcF84i5690kssWPhWRHmrxtas8Tjougl3Cfn64I18iQWfBJtgzAfqhKXVkD1mIc6TwHWrG40LWFpqSQEEZvPa9v3a05p6LIDvuex0ISIY_TFJ0iKVCr17jEWJicLfvoBGbCfEhImV0NeWhGwMQu8Vt5CfY85uuEkXf1Eit9UO8MdD_SlnSUuzXk549mD8w9IJWzjESa-ozntv39zVyUQxDhjHb1qXXn-wS4ALUaOU6NgG8NDbK2Ajw";
}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<DeveloperRepository>();
builder.Services.AddScoped<GenreRepository>();

builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<PaginateCollection>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateGameValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.UseSwagger();
    app.UseSwaggerUI();
}

string storagePath = Path.Combine(builder.Environment.ContentRootPath, "FileStorage");

if (!Directory.Exists(storagePath))
{
    Directory.CreateDirectory(storagePath);
}

string imagesPath = Path.Combine(storagePath, "images");

if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/images",
    FileProvider = new PhysicalFileProvider(imagesPath)
});

app.UseMiddleware<RequestTimingMiddleware>();        
app.UseMiddleware<RequestConsoleLoggingMiddleware>(); 
app.UseMiddleware<IpBlockingMiddleware>();      
app.UseMiddleware<RequestCounterMiddleware>();      
app.UseMiddleware<ApiKeyMiddleware>();             

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<TestMiddleware>();

app.MapControllers();

await app.SeedAsync();

app.Run();