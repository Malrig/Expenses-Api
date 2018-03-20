using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Models;

namespace ExpensesApi.ViewModels {
  /// <summary>
  /// ViewModel containing all the information for
  /// an expense (this includes its expense lines).
  /// </summary>
  public class ExpenseDetail {
    // Data directly from the model
    /// <summary>
    /// ID of the expense
    /// </summary>  
    public int expenseId { get;  }
    /// <summary>
    /// Expense name
    /// </summary>
    public string name { get;  }
    /// <summary>
    /// Date the expense was paid
    /// </summary>
    public DateTime billedDate { get;  }
    /// <summary>
    /// Date the expense should be recorded against
    /// </summary>
    public DateTime? effectiveDate { get;  }
    /// <summary>
    /// The expense lines the expense contains
    /// </summary>
    public virtual ICollection<ExpenseLine> expenseLines { get;  }

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
