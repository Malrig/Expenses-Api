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
  public class AddExpenseValidatorTests {
    private ExpenseContext expenseDb;
    protected IValidator addExpenseValidator;

    public AddExpenseValidatorTests() {
      expenseDb = DatabaseSetup.CreateExpenseContext();
      addExpenseValidator = new AddExpenseValidator(expenseDb);
    }

    public class ValidateMethod : AddExpenseValidatorTests {
      public ValidateMethod() : base() { }

      [Fact]
      public void ValidationSucceeds() {
        AddExpenseInfo command = new AddExpenseInfo();

        List<ValidationResult> results = addExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.Empty(results);
      }
    }
  }
}
