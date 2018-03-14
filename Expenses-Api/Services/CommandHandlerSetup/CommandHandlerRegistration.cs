using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExpensesApi.Validation;

namespace ExpensesApi.Services {
  public static class CommandHandlerRegistration {
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

    public static IServiceCollection RegisterCommandHandler<TCommandHandler, TCommand>(
       this IServiceCollection services)
        where TCommandHandler : class, ICommandHandler<TCommand> {

      services.AddScoped<TCommandHandler>();

      services.AddScoped<ICommandHandler<TCommand>>(x => x.GetService<TCommandHandler>());

      return services;
    }
  }
}
