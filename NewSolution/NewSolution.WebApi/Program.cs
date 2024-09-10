using Autofac.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using NewSolution.Service;
using NewSolution.Service.Common;
using NewSolution.WebApi.Controllers;
using NewSolution.Repository;
using NewSolution.Repository.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<FootballerService>().As<IFootballerService>().InstancePerDependency();
    containerBuilder.RegisterType<FootballerRepository>().As<IFootballerRepository>().InstancePerDependency();
    containerBuilder.RegisterType<ClubService>().As<IClubService>().InstancePerDependency();
    containerBuilder.RegisterType<ClubRepository>().As<IClubRepository>().InstancePerDependency();
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();
