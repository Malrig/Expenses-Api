using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Models {
  public class ExpenseLine {
    public int expenseLineId { get; set; }

    public string name { get; set; }
    public decimal amount { get; set; }
    public int expenseId { get; set; }

    // Foreign Objects
    public virtual Expense expense { get; set; }
  }
}
