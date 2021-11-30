using System;
using System.Net.Http;
using Application.Domain.ReadModel;
using Application.Domain.WriteModel.Commands;
using Application.Infrastructure.Commands;
using Application.Infrastructure.ES;
using Application.Infrastructure.InMemory;
using EventStore.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scheduling.Domain.Infrastructure.Commands;

namespace Application;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
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
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private EventStoreClient GetEventStoreClient()
    {
        return new EventStoreClient(new EventStoreClientSettings
        {
            ConnectivitySettings =
            {
                Address = new Uri("http://localhost:2113"),
            },
            DefaultCredentials = new UserCredentials("admin", "changeit")
        });
    }
}