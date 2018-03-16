using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.Expenses {
  public class UpdateExpense : ICommandHandler<UpdateExpenseInfo> {
    private ExpenseContext expenseDb;
    private ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense;

    public UpdateExpense(ExpenseContext expenseDb,
                            ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense) {
      this.expenseDb = expenseDb;
      this.addUpdateExpense = addUpdateExpense;
    }

    public void Handle(UpdateExpenseInfo command) {
      addUpdateExpense.Handle(command.GetAddUpdateInfo());

      expenseDb.SaveChanges();
    }
  }

  public class UpdateExpenseInfo {
    public int expenseId { get; set; }
    public string name { get; set; }
    public DateTime billedDate { get; set; }
    public DateTime? effectiveDate { get; set; }

    public List<ExpenseLine> expenseLines { get; set; }

    public bool expenseLinesIncluded { get; set; } = false;

    public UpdateExpenseInfo() { }

    public AddUpdateExpenseInfo GetAddUpdateInfo() {
      return new AddUpdateExpenseInfo() {
        // When updateing use an ID of 0 and always include
        // the expense lines.
        expenseId = expenseId,
        name = name,
        billedDate = billedDate,
        effectiveDate = effectiveDate,
        expenseLines = expenseLines,
        expenseLinesIncluded = expenseLinesIncluded
      };
    }
  }
}
