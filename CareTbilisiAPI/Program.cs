using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using CareTbilisiAPI.Application.Services;
using CareTbilisiAPI.Domain.Interfaces;
using CareTbilisiAPI.Domain.Interfaces.Repositories;
using CareTbilisiAPI.Domain.Interfaces.Services;
using CareTbilisiAPI.Domain.Models;
using CareTbilisiAPI.Infrastructure.Repositories;
using CareTbilisiAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<DatabaseSettings>(
      builder.Configuration.GetSection(nameof(DatabaseSettings))
    );

builder.Services.AddSingleton<IDatabaseSettings>(obj => obj.GetRequiredService<IOptions<DatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));


string jsonFilePath = @"./appsettings.Development.json";

if (File.Exists(jsonFilePath))
{
    string jsonData = File.ReadAllText(jsonFilePath);

    JObject config = JObject.Parse(jsonData);

    var mongoDbIdentityCongif = new MongoDbIdentityConfiguration()
    {

         MongoDbSettings =  new MongoDbSettings() 
         {
            ConnectionString = config["DatabaseSettings"]?["ConnectionString"]?.Value<string>(),
            DatabaseName = config["DatabaseSettings"]?["DatabaseName"]?.Value<string>()
         },

         IdentityOptionsAction = options =>
         {
             options.Password.RequireDigit = false;
             options.Password.RequiredLength = 8;
             options.Password.RequireNonAlphanumeric = true;
             options.Password.RequireLowercase = false;  

             // lockout
             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(10);
             options.Lockout.MaxFailedAccessAttempts = 5;

             options.User.RequireUniqueEmail = true;

         }
    };

    builder.Services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentityCongif)
        .AddUserManager<UserManager<ApplicationUser>>()
        .AddSignInManager<SignInManager<ApplicationUser>>()
        .AddRoleManager<RoleManager<ApplicationRole>>();
}

    
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddTransient<IItemRepository, ItemRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
