using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Validation;

namespace ExpensesApi.Helpers {
  /// <summary>
  /// Object which is returned by controllers when an
  /// exception is hit by a request.
  /// </summary>
  public class ErrorInfo {
    /// <summary>
    /// Whether the exception indicates a complete failure
    /// or not.
    /// </summary>
    public bool warningOnly { get; }
    /// <summary>
    /// A summary of the error
    /// </summary>
    public string errorMessage { get; }
    /// <summary>
    /// The stack trace of the error
    /// </summary>
    public string stackTrace { get; }
    /// <summary>
    /// The validation errors which were hit by the request.
    /// </summary>
    public List<ValidationResult> validationErrors { get; }

    /// <summary>
    /// Default constructor, consumes an exception and extracts
    /// the different properties based on its type.
    /// </summary>
    /// <param name="exception"></param>
    public ErrorInfo(Exception exception) {
      if (exception is ValidationException) {
        ValidationException validationEx = (ValidationException)exception;
        validationErrors = new List<ValidationResult>();

        warningOnly = true;
        errorMessage = "Validation errors were hit while performing the request.";
        // Collect all the validation errors into the list
        foreach (ValidationResult result in validationEx.Errors) {
          validationErrors.Add(result);
        }
      }
      else {
        errorMessage = exception.GetBaseException().Message;
        stackTrace = exception.StackTrace;
        warningOnly = false;
      }
    }
  }
}
