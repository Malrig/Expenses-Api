using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Models;

namespace ExpensesApi.ViewModels {
  public class ExpenseDetail {
    // Data directly from the model
    public int expenseId { get; }
    public string name { get; }
    public DateTime billedDate { get; }
    public DateTime? effectiveDate { get; }
    public virtual ICollection<ExpenseLine> expenseLines { get; }

    // View model data
    public decimal totalAmount { get; }

    public ExpenseDetail(Expense expense) {
      this.expenseId = expense.expenseId;
      this.name = expense.name;
      this.billedDate = expense.billedDate;
      this.effectiveDate = expense.effectiveDate;
      this.expenseLines = new List<ExpenseLine>();

      this.totalAmount = 0;
      foreach (ExpenseLine expenseLine in expense.expenseLines) {
        this.expenseLines.Add(expenseLine);
        this.totalAmount += expenseLine.amount;
      }
    }
  }
}
