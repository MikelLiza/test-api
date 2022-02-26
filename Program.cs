using APITest;
using APITest.Models;
using APITest.Repositories;
using APITest.Services;
using APITest.Settings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOptions();
builder.Services.Configure<FirebaseConfig>(builder.Configuration.GetSection("FirebaseConfig"));
builder.Services.Configure<TwitchConfig>(builder.Configuration.GetSection("TwitchConfig"));
builder.Services.Configure<IGDBConfig>(builder.Configuration.GetSection("IGDBConfig"));
builder.Configuration.AddJsonFile("appsettings.Development.json", true, true);
builder.Services.AddDbContext<TestDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("TestDb")));
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "APITest", Version = "v1"}); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Initialize services in /Services
builder.Services.AddTransient<ISettingsService, SettingsService>();
builder.Services.AddTransient<IItemsService, ItemsService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IIGDBService, IGDBService>();
builder.Services.AddTransient<IFakeStoreService, FakeStoreService>();

// Initialize repositories in /Repositories
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient(typeof(IExternalRepository<>), typeof(ExternalRepository<>));
builder.Services.AddTransient<IIGDBRepository, IGDBRepository>();
builder.Services.AddTransient<IFirebaseRepository, FirebaseRepository>();

var mapperConfig = new MapperConfiguration(amc => { amc.AddProfile(new AutoMapperProfile()); });

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "APITest v1"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();