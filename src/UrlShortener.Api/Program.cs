using UrlShortener.Infrastructure;
using UrlShortener.Application;
using UrlShortener.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Serves the OpenAPI JSON
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlShortener API V1");
        c.RoutePrefix = "swagger"; // This makes Swagger UI load at the root (http://localhost:5000/)
    });
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();