using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Models;

namespace ExpensesApi.ViewModels {
  public class SimpleExpense {
    // Copied directly from models
    public int expenseId { get; set; }
    public string name { get; set; }
    public DateTime billedDate { get; set; }
    public DateTime? effectiveDate { get; set; }

    // ViewModel data
    public decimal totalAmount { get; set; }

    public SimpleExpense(Expense toReturn) {
      this.expenseId = toReturn.expenseId;
      this.name = toReturn.name;
      this.billedDate = toReturn.billedDate;
      this.effectiveDate = toReturn.effectiveDate;
      this.totalAmount = 0;

      foreach (ExpenseLine expenseLine in toReturn.expenseLines){
        totalAmount += expenseLine.amount;
      }
    }
  }
}
