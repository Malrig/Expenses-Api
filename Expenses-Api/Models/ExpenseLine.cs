using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Models {
  /// <summary>
  /// Expense Line
  /// </summary>
  public class ExpenseLine {
    /// <summary>
    /// ID of the expense line
    /// </summary>
    public int expenseLineId { get; set; }

    /// <summary>
    /// Expense line name
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// The amount associated with this line
    /// </summary>
    public decimal amount { get; set; }
    /// <summary>
    /// The expense this line is associated with
    /// </summary>
    public int expenseId { get; set; }
    
    public virtual Expense expense { get; set; }
  }
}
