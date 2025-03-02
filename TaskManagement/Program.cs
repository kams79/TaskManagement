using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagement.Business.Models;
using TaskManagement.Business.Validators;
using TaskManagement.DataAccess.DbContexts;
using TaskManagement.DataAccess.Services;
using TaskManagement.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        // using System.Reflection;
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
);

builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IValidator<TaskItemDto>, TaskValidator>();

builder.Services.AddDbContext<TaskItemContext>(options =>
    options
    .UseInMemoryDatabase("DataBase"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskItemContext>();
    SeedData.Initialize(context);
}

app.Run();

/// <summary>
/// The main entry point for the application.
/// </summary>
public partial class Program { }
