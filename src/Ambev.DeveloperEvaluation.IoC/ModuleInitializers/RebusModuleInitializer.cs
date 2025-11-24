using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.RabbitMq;
using Serilog;
using Rebus.Logging;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

/// <summary>
/// Module initializer for Rebus message bus configuration (Producer only)
/// </summary>
public class RebusModuleInitializer : IModuleInitializer
{
    /// <summary>
    /// Initializes Rebus configuration with RabbitMQ transport as one-way client (producer only)
    /// </summary>
    public void Initialize(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        
        try
        {
            var rebusConnectionString = configuration["Rebus:ConnectionString"] 
                ?? throw new InvalidOperationException("Rebus:ConnectionString configuration is required");
            var queueName = configuration["Rebus:QueueName"] 
                ?? throw new InvalidOperationException("Rebus:QueueName configuration is required");

            Log.Information("Initializing Rebus as ONE-WAY CLIENT (producer only). Target Queue: {QueueName}", queueName);

            // Register Rebus as one-way client (producer only - no message consumption)
            builder.Services.AddRebus(configure => configure
                .Logging(l => l.Serilog(Serilog.Log.Logger))
                .Transport(t => t.UseRabbitMqAsOneWayClient(rebusConnectionString))
                .Routing(r => r.TypeBased()
                    .Map<Domain.Events.SaleCreatedEvent>(queueName)
                    .Map<Domain.Events.SaleCancelledEvent>(queueName)
                    .Map<Domain.Events.SaleModifiedEvent>(queueName)
                    .Map<Domain.Events.ItemCancelledEvent>(queueName)
                    .Map<Domain.Events.UserRegisteredEvent>(queueName))
            );

            Log.Information("Rebus configured as producer only. Events will be published to queue: {QueueName}", queueName);
            Log.Information("Events configured: SaleCreatedEvent, SaleCancelledEvent, SaleModifiedEvent, ItemCancelledEvent, UserRegisteredEvent");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize Rebus configuration");
            throw;
        }
    }
}

