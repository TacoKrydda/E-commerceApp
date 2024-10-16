using E_commerceClassLibrary.Context;
using E_commerceClassLibrary.Interfaces.Production;
using E_commerceClassLibrary.Services.Production;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<E_commerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISizeService, SizeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
