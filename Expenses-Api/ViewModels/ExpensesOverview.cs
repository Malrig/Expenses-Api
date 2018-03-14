using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Models;

namespace ExpensesApi.ViewModels {
  public class ExpensesOverview {
    public List<ExpenseOverview> expenses;

    public ExpensesOverview(List<Expense> expenses) {
      this.expenses = new List<ExpenseOverview>();

      foreach (Expense expense in expenses) {
        this.expenses.Add(new ExpenseOverview(expense));
      }
    }

    public class ExpenseOverview {
      // Data directly from the model
      public int expenseId { get; }
      public string name { get; }
      public DateTime billedDate { get; }
      public DateTime? effectiveDate { get; }

      // View model data
      public decimal totalAmount { get; }

      public ExpenseOverview(Expense expense) {
        this.expenseId = expense.expenseId;
        this.name = expense.name;
        this.billedDate = expense.billedDate;
        this.effectiveDate = expense.effectiveDate;

        this.totalAmount = 0;
        foreach (ExpenseLine expenseLine in expense.expenseLines) {
          this.totalAmount += expenseLine.amount;
        }
      }
    }
  }
}
