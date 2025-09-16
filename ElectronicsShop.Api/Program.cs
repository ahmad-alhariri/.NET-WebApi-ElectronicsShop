using ElectronicsShop.Api;
using ElectronicsShop.Application;
using ElectronicsShop.Application.Middlewares;
using ElectronicsShop.Domain;
using ElectronicsShop.Infrastructure;
using ElectronicsShop.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddPresentation(builder.Configuration)
    .AddDomain()
    .AddApplication()
    .AddInfrastructure()
    .AddPersistence(builder.Configuration);
    

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Electronics Shop API V1");

        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.EnableFilter();
    });
    app.MapScalarApiReference();
    
}
else
{
    app.UseHsts();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();