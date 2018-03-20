using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExpensesApi.Validation;

namespace ExpensesApi.Services {
  /// <summary>
  /// Class which allows registering commands as a service
  /// with a validator.
  /// </summary>
  public static class CommandHandlerRegistration {
    /// <summary>
    /// Function to register a command as a service with an 
    /// associated validator.
    /// </summary>
    /// <typeparam name="TCommandHandler"></typeparam>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TValidator"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterCommandHandler<TCommandHandler, TCommand, TValidator>(
        this IServiceCollection services)
        where TCommandHandler : class, ICommandHandler<TCommand>
        where TValidator : Validator<TCommand> {

      services.AddScoped<TCommandHandler>();
      services.AddScoped<TValidator>();

      services.AddScoped<ICommandHandler<TCommand>>(x =>
          new ValidationCommandHandlerDecorator<TCommand>(x.GetService<TCommandHandler>(), x.GetService<TValidator>()));

      return services;
    }

    /// <summary>
    /// Function to register a command as a service without
    /// an associated validator.
    /// </summary>
    /// <typeparam name="TCommandHandler"></typeparam>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterCommandHandler<TCommandHandler, TCommand>(
       this IServiceCollection services)
        where TCommandHandler : class, ICommandHandler<TCommand> {

      services.AddScoped<TCommandHandler>();

      services.AddScoped<ICommandHandler<TCommand>>(x => x.GetService<TCommandHandler>());

      return services;
    }
  }
}
