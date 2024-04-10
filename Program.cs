using AspNetCore.Identity.MongoDbCore.Infrastructure;
using auth_backend.DAL.Model;
using auth_backend.Middlewares;
using auth_backend.mongo;
using auth_backend.services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Connection string for mongodb

//var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();//El GetSection(nameof(MongoDbConfig)) busca el nombre de la clase en el app settings y el Get lo convierte al tipo
//var asd = builder.Configuration.GetSection("MongoDbConfig");
//builder.Services.Configure<MongoDbConfig>( builder.Configuration.GetSection("MongoDbConfig") );

//builder.Services.AddSingleton<IMongoClient>(sp =>
//{
//    var config = sp.GetRequiredService<IOptions<MongoDbConfig>>().Value;
//    return new MongoClient(config.ConnectionString);
//});

var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.ConfigureCors();

//builder.Services.AddDbContext<CarBookingDbContext>(options =>
//options.UseMongoDB(mongoDBSettings.AtlasURI ?? "", mongoDBSettings.DatabaseName ?? ""));


////Se agrega Identity
//builder.Services.AddIdentity<User, Roles>()
//    .AddMongoDbStores<User, Roles, Guid>( mongoDbSettings.ConnectionString , mongoDbSettings.Name );

////Se Agrega AutoMapper
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
