using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Models {
  public class Expense {
    public int expenseId { get; set; }
    
    public string name { get; set; }
    public DateTime billedDate { get; set; }
    public DateTime? effectiveDate { get; set; }

    // Foreign Objects
    public virtual ICollection<ExpenseLine> expenseLines { get; set; }

    public Expense() { }
  }
}
