using System.Text.Json.Serialization;
using AutoMapper;
using LifeEcommerce.Helpers;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Services.IServices;
using MultiLanguageExamManagementSystem.Services;
using Serilog;
using MultiLanguageExamManagementSystem.Helpers;

var builder = WebApplication.CreateBuilder(args);

var mapperConfiguration = new MapperConfiguration(
                        mc => mc.AddProfile(new AutoMapperConfigurations()));
// builder.Services.AddLogging(); //added

IMapper mapper = mapperConfiguration.CreateMapper();

builder.Services.AddSingleton(mapper);


// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IExamService, ExamService>();

// builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ICultureService, CultureService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TranslationService>();

builder.Services.AddSingleton(builder.Configuration);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<CultureMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
