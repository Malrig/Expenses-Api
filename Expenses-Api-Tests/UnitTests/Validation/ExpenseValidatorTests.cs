using Xunit;
using System;
using System.Collections.Generic;

using ExpensesApi.Validation;
using ExpensesApi.Models;

namespace ExpensesApiTests.ValidationTests {
  public class ExpenseValidatorTests {
    private IValidator expenseValidator;
    protected Expense completedExpense;
    protected List<Expense> completedExpenses;

    public ExpenseValidatorTests() {
      expenseValidator = new ExpenseValidator();

      completedExpense = new Expense() {
        name = "Completed Expense",
        billedDate = DateTime.Parse("1 June 2017"),
        effectiveDate = DateTime.Parse("1 August 2017"),
        expenseLines = new List<ExpenseLine>()
      };

      completedExpense.expenseLines.Add(new ExpenseLine() {
        name = "Expense Line",
        amount = new decimal(3.50f)
      });

      completedExpenses = new List<Expense> {
        completedExpense,
        completedExpense,
        completedExpense
      };
    }

    public class ValidateMethod : ExpenseValidatorTests {
      public ValidateMethod() : base() { }

      [Fact]
      public void ValidateCompleteObject() {
        var ex = Record.Exception(() => expenseValidator.Validate(completedExpense));

        // Assert
        Assert.Null(ex);
      }

      [Fact]
      public void ValidateObjectNoName() {
        completedExpense.name = "";
        
        var ex = Record.Exception(() => expenseValidator.Validate(completedExpense));

        // Assert
        Assert.IsType<ValidationException>(ex);
        ValidationException validationError = (ValidationException)ex;
        Assert.Contains(new ValidationResult("Name", "Name is required."),
                        validationError.ValidationErrors);
      }

      [Fact]
      public void ValidateObjectNoBilledDate() {
        completedExpense.billedDate = DateTime.MinValue;
        
        var ex = Record.Exception(() => expenseValidator.Validate(completedExpense));

        // Assert
        Assert.IsType<ValidationException>(ex);
        ValidationException validationError = (ValidationException)ex;
        Assert.Contains(new ValidationResult("billedDate", "A billed date is required."),
                        validationError.ValidationErrors);
      }

      [Fact]
      public void ValidateObjectNoEffectiveDate() {
        completedExpense.effectiveDate = null;
        
        var ex = Record.Exception(() => expenseValidator.Validate(completedExpense));

        // Assert
        Assert.Null(ex);
      }

      [Fact]
      public void ValidateObjectNoExpenseLines() {
        completedExpense.expenseLines = null;

        var ex = Record.Exception(() => expenseValidator.Validate(completedExpense));

        // Assert
        Assert.IsType<ValidationException>(ex);
        ValidationException validationError = (ValidationException)ex;
        Assert.Contains(new ValidationResult("expenseLines", "An expense must have at least one expense line."),
                        validationError.ValidationErrors);
      }
    }

    public class ValidateAllMethod : ExpenseValidatorTests {
      public ValidateAllMethod() : base() { }

      [Fact]
      public void ValidateCompleteObjects() {
        var ex = Record.Exception(() => expenseValidator.ValidateAll(completedExpenses));

        // Assert
        Assert.Null(ex);
      }

      [Fact]
      public void ValidateObjectNoName() {
        completedExpenses[1].name = "";

        var ex = Record.Exception(() => expenseValidator.ValidateAll(completedExpenses));

        // Assert
        Assert.IsType<ValidationException>(ex);
        ValidationException validationError = (ValidationException)ex;
        Assert.Contains(new ValidationResult("Name", "Name is required."),
                        validationError.ValidationErrors);
      }

      [Fact]
      public void ValidateObjectNoBilledDate() {
        completedExpenses[1].billedDate = DateTime.MinValue;

        var ex = Record.Exception(() => expenseValidator.ValidateAll(completedExpenses));

        // Assert
        Assert.IsType<ValidationException>(ex);
        ValidationException validationError = (ValidationException)ex;
        Assert.Contains(new ValidationResult("billedDate", "A billed date is required."),
                        validationError.ValidationErrors);
      }

      [Fact]
      public void ValidateObjectNoEffectiveDate() {
        completedExpenses[1].effectiveDate = null;

        var ex = Record.Exception(() => expenseValidator.ValidateAll(completedExpenses));

        // Assert
        Assert.Null(ex);
      }

      [Fact]
      public void ValidateObjectNoExpenseLines() {
        completedExpenses[1].expenseLines = null;

        var ex = Record.Exception(() => expenseValidator.ValidateAll(completedExpenses));

        // Assert
        Assert.IsType<ValidationException>(ex);
        ValidationException validationError = (ValidationException)ex;
        Assert.Contains(new ValidationResult("expenseLines", "An expense must have at least one expense line."),
                        validationError.ValidationErrors);
      }
    }
  }
}