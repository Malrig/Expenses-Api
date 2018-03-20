using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Services {
  /// <summary>
  /// Interface for the command classes, they
  /// expose a single function Handle which 
  /// performs the command
  /// </summary>
  /// <typeparam name="TCommand"></typeparam>
  public interface ICommandHandler<TCommand> {
    /// <summary>
    /// Function which performs the actual command,
    /// all information for the command should be included
    /// in the TCommand class
    /// </summary>
    /// <param name="command"></param>
    void Handle(TCommand command);
  }
}
