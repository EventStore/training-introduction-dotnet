using System;
using Application.Application;
using Application.Domain.ReadModel;
using Application.Domain.WriteModel.Commands;
using Application.Infrastructure.Commands;
using Application.Infrastructure.ES;
using Application.Infrastructure.InMemory;
using Application.Infrastructure.Projections;
using Application.Infrastructure.Projections.Scheduling.Domain.Infrastructure.Projections;
using EventStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Scheduling.Domain.Infrastructure.Commands;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers().AddNewtonsoftJson();

var client = GetEventStoreClient();

var eventStore = new EsEventStore(client);
var aggregateStore = new EsAggregateStore(eventStore);
var handlers = new Handlers(aggregateStore);

services.AddSingleton(client);
services.AddSingleton(new CommandHandlerMap(handlers));
services.AddSingleton<Dispatcher>();
services.AddSingleton<IPatientSlotsRepository, InMemoryPatientSlotsRepository>();
services.AddSingleton<IAvailableSlotsRepository, InMemoryAvailableSlotsRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Patients API", Description = "API for booking doctor appointments", Version = "v1" });
});

var app = builder.Build();
        var patientSlotsRepository = new InMemoryPatientSlotsRepository();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patients API");
});

var availableSlotsRepository = new InMemoryAvailableSlotsRepository();

var subManager = new SubscriptionManager(
    client,
    "Slots",
    StreamName.AllStream,
    new Projector(new AvailableSlotsProjection(availableSlotsRepository)),
    new Projector(new PatientSlotsProjection(patientSlotsRepository))
);

subManager.Start();

app.Run();

static EventStoreClient GetEventStoreClient() =>
    new(EventStoreClientSettings.Create("esdb://localhost:2113?tls=false"));