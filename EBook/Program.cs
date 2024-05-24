using EBook.Programcsmetods;
using EBook.Context;
using EBook.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EBook.Service;
using EBook.Services.Interf;
using EBook.Services.Iface;
using EBook.Services.JWTDetails;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.Configure<JWTClaimsDetails>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddDbContext<JwtContext>(Options =>
  Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ILoginService, Login>();
builder.Services.AddSingleton<IBookService, DataBaseManager>();
builder.Services.AddSingleton<IAuthorService, AuthorDataBaseM>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.SwaggerAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
