using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Validation;

namespace ExpensesApi.Services {
  public class ValidationCommandHandlerDecorator<T> : ICommandHandler<T> {
    private readonly ICommandHandler<T> decoratee;
    private readonly IValidator validator;

    public ValidationCommandHandlerDecorator(
        ICommandHandler<T> decoratee, IValidator validator) {
      this.decoratee = decoratee;
      this.validator = validator;
    }

    public void Handle(T command) {
      var errors = this.validator.Validate(command).ToArray();

      if (errors.Any()) {
        throw new ValidationException(errors);
      }

      this.decoratee.Handle(command);
    }
  }
}
