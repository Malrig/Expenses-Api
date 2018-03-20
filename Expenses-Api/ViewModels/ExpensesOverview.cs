using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Models;

namespace ExpensesApi.ViewModels {
  /// <summary>
  /// ViewModel containing an overview of all expenses.
  /// This does not include detailed information such
  /// as expense lines.
  /// </summary>
  public class ExpensesOverview {
    /// <summary>
    /// The list of expenses
    /// </summary>
    public List<ExpenseOverview> expenses { get; }


    /// <summary>
    /// Constructor which pulls all of the required 
    /// information out of the expenses and nothing more.
    /// </summary>
    /// <param name="expenses"></param>
    public ExpensesOverview(List<Expense> expenses) {
      this.expenses = new List<ExpenseOverview>();

      foreach (Expense expense in expenses) {
        this.expenses.Add(new ExpenseOverview(expense));
      }
    }

    /// <summary>
    /// ViewModel containing all the information for
    /// an overview of an expense.
    /// </summary>
    public class ExpenseOverview {
      // Data directly from the model
      /// <summary>
      /// ID of the expense
      /// </summary>  
      public int expenseId { get; }
      /// <summary>
      /// Expense name
      /// </summary>
      public string name { get; }
      /// <summary>
      /// Date the expense was paid
      /// </summary>
      public DateTime billedDate { get; }
      /// <summary>
      /// Date the expense should be recorded against
      /// </summary>
      public DateTime? effectiveDate { get; }

      // View model data
      /// <summary>
      /// The total amount for this expense equal to the
      /// sum of the amounts for all the expense lines.
      /// </summary>
      public decimal totalAmount { get; }

      /// <summary>
      /// Constructor which pulls all of the required 
      /// information out of the expense.
      /// </summary>
      /// <param name="expense"></param>
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
