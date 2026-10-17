using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using VideoGames.BLL.Settings;
using VideoGames.BLL.Validators.Game;
using VideoGames.DAL;
using VideoGames.DAL.Initializer;
using VideoGames.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("LocalDb");
    if (connectionString == null)
    {
        throw new Exception("Connection string 'LocalDb' not found in configuration.");
    }
    options.UseNpgsql(connectionString);
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/.log", rollingInterval: RollingInterval.Hour)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

string corsName = "allowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsName, cfg =>
    {
        cfg.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODE3OTQyNDAwIiwiaWF0IjoiMTc4NjQ0NjkxOCIsImFjY291bnRfaWQiOiIwMTk5NTEzZTdlYmY3YjYwOGI4Y2I3NTI3YTE3ZTI5MyIsImN1c3RvbWVyX2lkIjoiMDE5OTUxM2U3ZWJmN2I2MDhiOGNiNzUyN2ExN2UyOTMiLCJzdWJfaWQiOiItIiwiZWRpdGlvbiI6IjAiLCJ0eXBlIjoiMiJ9.gnQYP7aLCcVQ_aS_g36BR2TVz1srfcCr3P5xrAw-1S6MNPECaqNweRUZCwbe6OKG6QL64wtDIYoFmuchoaQSmtAXDRldrVvsOcF84i5690kssWPhWRHmrxtas8Tjougl3Cfn64I18iQWfBJtgzAfqhKXVkD1mIc6TwHWrG40LWFpqSQEEZvPa9v3a05p6LIDvuex0ISIY_TFJ0iKVCr17jEWJicLfvoBGbCfEhImV0NeWhGwMQu8Vt5CfY85uuEkXf1Eit9UO8MdD_SlnSUuzXk549mD8w9IJWzjESa-ozntv39zVyUQxDhjHb1qXXn-wS4ALUaOU6NgG8NDbK2Ajw";
}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddRepositories();

builder.Services.AddServices();

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

app.AddStaticFiles(app.Environment);

app.UseCors(corsName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.SeedAsync();

app.Run();