using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.ExpenseLines;
using ExpensesApi.Validation;
using ExpensesApi.Validation.ExpenseLines;

using ExpensesApiTests.TestCommon;

namespace ExpensesApiTests.UnitTests.Validation.ExpenseLines {
  public class AddUpdateExpenseLineValidatorTests {
    private ExpenseContext expenseDb;
    protected IValidator addUpdateExpenseLineValidator;

    public AddUpdateExpenseLineValidatorTests() {
      expenseDb = DatabaseSetup.CreateExpenseContext();
      addUpdateExpenseLineValidator = new AddUpdateExpenseLineValidator(expenseDb);

      expenseDb.Expenses.Add(new Expense() {
        expenseId = 1
      });
      expenseDb.ExpenseLines.Add(new ExpenseLine() {
        expenseLineId = 1,
        expenseId = 1
      });

      expenseDb.SaveChanges();
    }

    public class ValidateMethod : AddUpdateExpenseLineValidatorTests {
      public ValidateMethod() : base() { }

      [Fact]
      public void AddingValidationSucceeds() {
        AddUpdateExpenseLineInfo command = new AddUpdateExpenseLineInfo() {
          expenseLineId = 0,
          expenseId = 1,
          name = "New Expense Line"
        };

        List<ValidationResult> results = addUpdateExpenseLineValidator.Validate(command).ToList();

        // Assert
        Assert.Empty(results);
      }

      [Fact]
      public void UpdatingValidationSucceeds() {
        AddUpdateExpenseLineInfo command = new AddUpdateExpenseLineInfo() {
          expenseLineId = 1,
          expenseId = 1,
          name = "Updated Expense Line"
        };

        List<ValidationResult> results = addUpdateExpenseLineValidator.Validate(command).ToList();

        // Assert
        Assert.Empty(results);
      }

      [Fact]
      public void ExpenseLineMustExist() {
        AddUpdateExpenseLineInfo command = new AddUpdateExpenseLineInfo() {
          expenseLineId = 2,
          expenseId = 1,
          name = "Updated Expense Line"
        };

        List<ValidationResult> results = addUpdateExpenseLineValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("ID",
                                             "No expense line exists with the ID: 2"),
                        results);
      }

      [Fact]
      public void ExpenseMustExist() {
        AddUpdateExpenseLineInfo command = new AddUpdateExpenseLineInfo() {
          expenseLineId = 1,
          expenseId = 2,
          name = "Updated Expense Line"
        };

        List<ValidationResult> results = addUpdateExpenseLineValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("ExpenseID",
                                             "No expense exists with the ID: 2"),
                        results);
      }

      [Fact]
      public void ExpenseLineNameIsRequired() {
        AddUpdateExpenseLineInfo command = new AddUpdateExpenseLineInfo() {
          expenseLineId = 1,
          expenseId = 1,
          name = null
        };

        List<ValidationResult> results = addUpdateExpenseLineValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("Name",
                                             "Name is required on an expense line"),
                        results);
      }

      [Fact]
      public void ExpenseLineNameCannotBeBlank() {
        AddUpdateExpenseLineInfo command = new AddUpdateExpenseLineInfo() {
          expenseLineId = 1,
          expenseId = 1,
          name = "      "
        };

        List<ValidationResult> results = addUpdateExpenseLineValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("Name",
                                             "Name is required on an expense line"),
                        results);
      }
    }
  }
}
