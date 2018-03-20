using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {
  /// <summary>
  /// Exception which is raised whenever a validation error
  /// is hit.
  /// </summary>
  public class ValidationException : Exception {
    /// <summary>
    /// Default constructor, it consumes a list of validation
    /// errors which can then be displayed to customers.
    /// </summary>
    /// <param name="r"></param>
    public ValidationException(IEnumerable<ValidationResult> r)
        : base(GetFirstErrorMessage(r)) {
      this.Errors =
          new ReadOnlyCollection<ValidationResult>(r.ToArray());
    }

    /// <summary>
    /// The list of errors.
    /// </summary>
    public ReadOnlyCollection<ValidationResult> Errors { get; private set; }

    private static string GetFirstErrorMessage(
        IEnumerable<ValidationResult> errors) {
      return errors.First().Message;
    }
  }
}
