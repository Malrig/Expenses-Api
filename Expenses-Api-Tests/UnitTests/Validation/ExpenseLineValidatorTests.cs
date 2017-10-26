using Xunit;
using System;
using System.Collections.Generic;

using ExpensesApi.Validation;
using ExpensesApi.Models;

namespace ExpenseLinesApiTests.ValidationTests {
  public class ExpenseLineValidatorTests {
    private IValidator ExpenseLineValidator;
    protected ExpenseLine completedExpenseLine;

    public ExpenseLineValidatorTests() {
      ExpenseLineValidator = new ExpenseLineValidator();

      completedExpenseLine = new ExpenseLine() {
        name = "Completed ExpenseLine"
      };
    }

    public class PerformValidationMethod : ExpenseLineValidatorTests {
      public PerformValidationMethod() : base() { }

      [Fact]
      public void ValidateCompleteObject() {
        var ex = Record.Exception(() => ExpenseLineValidator.Validate(completedExpenseLine));

        // Assert
        Assert.Null(ex);
      }

      [Fact]
      public void ValidateObjectNoName() {
        completedExpenseLine.name = "";
        
        var ex = Record.Exception(() => ExpenseLineValidator.Validate(completedExpenseLine));

        // Assert
        Assert.IsType<ValidationException>(ex);
        ValidationException validationError = (ValidationException)ex;
        Assert.Contains(new ValidationResult("Name", "Name is required."),
                        validationError.ValidationErrors);
      }
    }
  }
}