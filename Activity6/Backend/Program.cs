using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the database context service.
builder.Services.AddDbContext<ShoppingDatabase>(options =>
    options.UseSqlServer("<YOUR CONNECTION STRING>"));

// Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Dev",
                      builder =>
                      {
                          builder.AllowAnyOrigin().AllowAnyMethod();
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use or configured cors rules.
app.UseCors("Dev");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
