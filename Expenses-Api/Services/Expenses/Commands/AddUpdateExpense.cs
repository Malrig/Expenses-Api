﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.Services.ExpenseLines;

namespace ExpensesApi.Services.Expenses {
  public class AddUpdateExpense : ICommandHandler<AddUpdateExpenseInfo> {
    private ExpenseContext expenseDb;
    private ICommandHandler<AddUpdateExpenseLineInfo> addUpdateExpenseLine;
    private ICommandHandler<DeleteExpenseLineInfo> deleteExpenseLine;

    public AddUpdateExpense(ExpenseContext expenseDb,
                            ICommandHandler<AddUpdateExpenseLineInfo> addUpdateExpenseLine,
                            ICommandHandler<DeleteExpenseLineInfo> deleteExpenseLine) {
      this.expenseDb = expenseDb;
      this.addUpdateExpenseLine = addUpdateExpenseLine;
      this.deleteExpenseLine = deleteExpenseLine;
    }

    public void Handle(AddUpdateExpenseInfo command) {
      Expense existing = expenseDb.Expenses.Where(e => e.expenseId == command.expenseId)
                                                       .SingleOrDefault();

      if (existing == null) {
        expenseDb.Expenses.Add(command.GetExpense());
      }
      else {
        expenseDb.Entry(existing).CurrentValues.SetValues(command);

        // Only update the expense lines if they are specified
        if (command.expenseLinesIncluded) {
          // Check expense lines and delete any that no longer exist
          foreach (ExpenseLine existingLine in existing.expenseLines) {
            if (!command.expenseLines.Any(el => el.expenseLineId == existingLine.expenseLineId)) {
              deleteExpenseLine.Handle(new DeleteExpenseLineInfo(existingLine.expenseLineId));
            }
          }

          // Update and add any expense lines
          foreach (ExpenseLine lineToProcess in command.expenseLines) {
            addUpdateExpenseLine.Handle(new AddUpdateExpenseLineInfo(lineToProcess));
          }
        }
      }

      expenseDb.SaveChanges();
    }
  }

  public class AddUpdateExpenseInfo {
    public int expenseId { get; set; }
    public string name { get; set; }
    public DateTime billedDate { get; set; }
    public DateTime? effectiveDate { get; set; }

    public List<ExpenseLine> expenseLines { get; set; }

    public bool expenseLinesIncluded {
      get => expenseLinesIncluded && (expenseLines != null);
      set => expenseLinesIncluded = value;
    }

    public AddUpdateExpenseInfo() { }

    public Expense GetExpense() {
      return new Expense() {
        expenseId = expenseId,
        name = name,
        billedDate = billedDate,
        effectiveDate = effectiveDate,
        expenseLines = expenseLines
      };
    }
  }
}
