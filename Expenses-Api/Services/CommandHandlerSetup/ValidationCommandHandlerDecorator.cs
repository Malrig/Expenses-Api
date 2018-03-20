using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Validation;

namespace ExpensesApi.Services {
  /// <summary>
  /// Class required for automatic validation on commands.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class ValidationCommandHandlerDecorator<T> : ICommandHandler<T> {
    private readonly ICommandHandler<T> decoratee;
    private readonly IValidator validator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="decoratee"></param>
    /// <param name="validator"></param>
    public ValidationCommandHandlerDecorator(
        ICommandHandler<T> decoratee, IValidator validator) {
      this.decoratee = decoratee;
      this.validator = validator;
    }

    /// <summary>
    /// Function which actually performs the validation.
    /// </summary>
    /// <param name="command"></param>
    public void Handle(T command) {
      var errors = this.validator.Validate(command).ToArray();

      if (errors.Any()) {
        throw new ValidationException(errors);
      }

      this.decoratee.Handle(command);
    }
  }
}
