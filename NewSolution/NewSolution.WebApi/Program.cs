using Autofac.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using NewSolution.Service;
using NewSolution.Service.Common;
using NewSolution.WebApi.Controllers;
using NewSolution.Repository;
using NewSolution.Repository.Common;

var builder = WebApplication.CreateBuilder(args);

// CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new ServiceDIModule());
    containerBuilder.RegisterModule(new RepositoryDIModule());
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
