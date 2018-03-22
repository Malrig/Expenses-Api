using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.Expenses;
using ExpensesApi.Validation;
using ExpensesApi.Validation.Expenses;

using ExpensesApiTests.TestCommon;

namespace ExpensesApiTests.UnitTests.Validation.Expenses {
  public class UpdateExpenseValidatorTests {
    private ExpenseContext expenseDb;
    protected IValidator updateExpenseValidator;

    public UpdateExpenseValidatorTests() {
      expenseDb = DatabaseSetup.CreateExpenseContext();
      updateExpenseValidator = new UpdateExpenseValidator(expenseDb);

      expenseDb.Expenses.Add(new Expense() {
        expenseId = 1
      });

      expenseDb.SaveChanges();
    }

    public class ValidateMethod : UpdateExpenseValidatorTests {
      public ValidateMethod() : base() { }

      [Fact]
      public void ValidationSucceeds() {
        UpdateExpenseInfo command = new UpdateExpenseInfo() {
          expenseId = 1
        };

        List<ValidationResult> results = updateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.Empty(results);
      }

      [Fact]
      public void ExpenseMustExist() {
        UpdateExpenseInfo command = new UpdateExpenseInfo() {
          expenseId = 0
        };

        List<ValidationResult> results = updateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("ID",
                                             "No expense exists with the ID: 0"),
                        results);
      }
    }
  }
}
