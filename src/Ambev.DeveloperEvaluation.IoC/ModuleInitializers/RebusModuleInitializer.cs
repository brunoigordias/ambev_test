using Ambev.DeveloperEvaluation.Application.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.RabbitMq;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

/// <summary>
/// Module initializer for Rebus message bus configuration
/// </summary>
public class RebusModuleInitializer : IModuleInitializer
{
    /// <summary>
    /// Initializes Rebus configuration with RabbitMQ transport
    /// </summary>
    public void Initialize(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var rebusConnectionString = configuration["Rebus:ConnectionString"] 
            ?? throw new InvalidOperationException("Rebus:ConnectionString configuration is required");
        var queueName = configuration["Rebus:QueueName"] 
            ?? throw new InvalidOperationException("Rebus:QueueName configuration is required");

        // Register Rebus with automatic handler registration
        // builder.Services.AddRebus(configure => configure
        //     .Transport(t => t.UseRabbitMq(rebusConnectionString, queueName))
        //     .Routing(r => r.TypeBased()
        //         .Map<Domain.Events.SaleCreatedEvent>(queueName)
        //         .Map<Domain.Events.SaleCancelledEvent>(queueName)
        //         .Map<Domain.Events.SaleModifiedEvent>(queueName)
        //         .Map<Domain.Events.ItemCancelledEvent>(queueName)
        //         .Map<Domain.Events.UserRegisteredEvent>(queueName))
        // );

        builder.Services.AddRebus(configure => configure
        .Logging(l => l.Serilog())
        .Transport(t => t.UseRabbitMq(rebusConnectionString, queueName))
        .Routing(r => r.TypeBased()
            .Map<Domain.Events.SaleCreatedEvent>(queueName)
            .Map<Domain.Events.SaleCancelledEvent>(queueName)
            .Map<Domain.Events.SaleModifiedEvent>(queueName)
            .Map<Domain.Events.ItemCancelledEvent>(queueName)
            .Map<Domain.Events.UserRegisteredEvent>(queueName))
        );

        // Register Rebus handlers automatically from assembly
        builder.Services.AutoRegisterHandlersFromAssemblyOf<SaleCreatedEventHandler>();

        return builder;
    }
}

