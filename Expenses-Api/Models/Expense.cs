using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Models {
  /// <summary>
  /// Expense
  /// </summary>
  public class Expense {
    /// <summary>
    /// ID of the expense
    /// </summary>  
    public int expenseId { get; set; }

    /// <summary>
    /// Expense name
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// Date the expense was paid
    /// </summary>
    public DateTime billedDate { get; set; }
    /// <summary>
    /// Date the expense should be recorded against
    /// </summary>
    public DateTime? effectiveDate { get; set; }

    /// <summary>
    /// The expense lines the expense contains
    /// </summary>
    public virtual ICollection<ExpenseLine> expenseLines { get; set; }
    
    public Expense() { }
  }
}
