﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {
  public class ValidationException : Exception {
    public ValidationException(IEnumerable<ValidationResult> r) : base(GetFirstErrorMessage(r)) {
      this.ValidationErrors = new ReadOnlyCollection<ValidationResult>(r.ToArray());
    }

    public ReadOnlyCollection<ValidationResult> ValidationErrors { get; private set; }

    private static string GetFirstErrorMessage(
        IEnumerable<ValidationResult> errors) {
      return errors.First().message;
    }
  }
}
