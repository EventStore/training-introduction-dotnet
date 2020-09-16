using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Application;
using Application.Infrastructure.InMemory;
using Application.Infrastructure.Projections;
using Application.Infrastructure.Projections.Scheduling.Domain.Infrastructure.Projections;
using EventStore.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = GetEventStoreClient();

            var availableSlotsRepository = new InMemoryAvailableSlotsRepository();

            var subManager = new SubscriptionManager(
                client,
                "Slots",
                StreamName.AllStream,
                new Projector(new AvailableSlotsProjection(availableSlotsRepository))
                );
            subManager.Start();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static EventStoreClient GetEventStoreClient()
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
}
