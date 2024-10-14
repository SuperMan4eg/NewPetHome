using NewPetHome.Web.Middlewares;
using NewPetHome.Species.Applications;
using NewPetHome.Species.Infrastructure;
using NewPetHome.Species.Infrastructure.DbContexts;
using NewPetHome.Species.Presentation;
using NewPetHome.Volunteers.Application;
using NewPetHome.Volunteers.Infrastructure;
using NewPetHome.Volunteers.Infrastructure.DbContexts;
using NewPetHome.Volunteers.Presentation;
using NewPetHome.Web.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Services
    .AddSpeciesApplication()
    .AddSpeciesInfrastructure(builder.Configuration)
    .AddSpeciesPresentation()
    .AddVolunteersApplication()
    .AddVolunteersInfrastructure(builder.Configuration)
    .AddVolunteersPresentation();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSerilog();

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigration<VolunteersWriteDbContext>();
    await app.ApplyMigration<SpeciesWriteDbContext>();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();