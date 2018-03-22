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
  public class AddUpdateExpenseValidatorTests {
    private ExpenseContext expenseDb;
    protected IValidator addUpdateExpenseValidator;

    public AddUpdateExpenseValidatorTests() {
      expenseDb = DatabaseSetup.CreateExpenseContext();
      addUpdateExpenseValidator = new AddUpdateExpenseValidator(expenseDb);

      // Add an existing expense/expense line
      expenseDb.Expenses.Add(new Expense() {
        expenseId = 1
      });

      expenseDb.SaveChanges();
    }

    public class ValidateMethod : AddUpdateExpenseValidatorTests {
      public ValidateMethod() : base() { }

      [Fact]
      public void AddingValidationSucceeds() {
        AddUpdateExpenseInfo command = new AddUpdateExpenseInfo() {
          expenseId = 0,
          name = "New Expense",
          billedDate = new DateTime(2018, 2, 2) 
        };

        List<ValidationResult> results = addUpdateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.Empty(results);
      }

      [Fact]
      public void UpdatingValidationSucceeds() {
        AddUpdateExpenseInfo command = new AddUpdateExpenseInfo() {
          expenseId = 1,
          name = "Updated Expense",
          billedDate = new DateTime(2018, 2, 2)
        };

        List<ValidationResult> results = addUpdateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.Empty(results);
      }

      [Fact]
      public void ExpenseMustExist() {
        AddUpdateExpenseInfo command = new AddUpdateExpenseInfo() {
          expenseId = 2,
          name = "Updated Expense",
          billedDate = new DateTime(2018, 2, 2)
        };

        List<ValidationResult> results = addUpdateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("ID", 
                                             "No expense exists with the ID: 2"),
                        results);
      }

      [Fact]
      public void ExpenseNameIsRequired() {
        AddUpdateExpenseInfo command = new AddUpdateExpenseInfo() {
          expenseId = 0,
          name = null,
          billedDate = new DateTime(2018, 2, 2)
        };

        List<ValidationResult> results = addUpdateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("Name", 
                                             "Name is required on an expense item"),
                        results);
      }

      [Fact]
      public void ExpenseNameCannotBeBlank() {
        AddUpdateExpenseInfo command = new AddUpdateExpenseInfo() {
          expenseId = 0,
          name = "        ",
          billedDate = new DateTime(2018, 2, 2)
        };

        List<ValidationResult> results = addUpdateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("Name",
                                             "Name is required on an expense item"),
                        results);
      }

      [Fact]
      public void ExpenseBilledDateIsRequired() {
        AddUpdateExpenseInfo command = new AddUpdateExpenseInfo() {
          expenseId = 1,
          name = "Updated Expense"
        };

        List<ValidationResult> results = addUpdateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("BilledDate",
                                             "A billed date is required for an expense item"),
                        results);
      }

      [Fact]
      public void ExpenseBilledDateCannotBeMinimum() {
        AddUpdateExpenseInfo command = new AddUpdateExpenseInfo() {
          expenseId = 1,
          name = "Updated Expense",
          billedDate = DateTime.MinValue
        };

        List<ValidationResult> results = addUpdateExpenseValidator.Validate(command).ToList();

        // Assert
        Assert.NotEmpty(results);
        Assert.Single(results);
        Assert.Contains(new ValidationResult("BilledDate",
                                             "A billed date is required for an expense item"),
                        results);
      }
    }
  }
}
