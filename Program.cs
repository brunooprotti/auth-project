using auth_backend.Bussiness.Mappers;
using auth_backend.Bussiness.Repository;
using auth_backend.Bussiness.Repository.IRepository;
using auth_backend.DAL;
using auth_backend.Middlewares;
using auth_backend.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Se Agrega AutoMapper
builder.Services.AddAutoMapper(typeof(AuthMapper));
//builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddDbContext<ApplicationDbContext>( options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQL"));
});
var sql = builder.Configuration.GetConnectionString("ConnectionSQL");
builder.Services.ConfigureCors();

builder.Services.AddScoped<IUserRepository, UserRepository>();
////Se agrega Identity
//builder.Services.AddIdentity<User, Roles>()
//    .AddMongoDbStores<User, Roles, Guid>( mongoDbSettings.ConnectionString , mongoDbSettings.Name );



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
