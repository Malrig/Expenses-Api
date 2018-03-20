using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using ExpensesApi.Validation;

namespace ExpensesApi.Helpers {
  /// <summary>
  /// IActionResult object which accepts an exception.
  /// It returns the correct ObjectResult based on the 
  /// exceptions type.
  /// </summary>
  public class ExceptionActionResult : IActionResult {
    private readonly Exception Exception;

    /// <summary>
    /// Default constructor, consumes an exception.
    /// </summary>
    /// <param name="ex"></param>
    public ExceptionActionResult(Exception ex) {
      Exception = ex;
    }

    /// <summary>
    /// Function which returns the correct ObjectResult
    /// based on the exception.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task ExecuteResultAsync(ActionContext context) {
      ObjectResult objectResult;
      if (Exception is ValidationException) {
        objectResult = new BadRequestObjectResult(new ErrorInfo(Exception));
      }
      else if (Exception is KeyNotFoundException) {
        objectResult = new NotFoundObjectResult(new ErrorInfo(Exception));
      }
      else {
        objectResult = new ObjectResult(new ErrorInfo(Exception));
        objectResult.StatusCode = 500;
      }

      await objectResult.ExecuteResultAsync(context);
    }
  }
}
