using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.Expenses {
  public class AddExpense : ICommandHandler<AddExpenseInfo> {
    private ExpenseContext expenseDb;
    private ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense;

    public AddExpense(ExpenseContext expenseDb,
                            ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense) {
      this.expenseDb = expenseDb;
      this.addUpdateExpense = addUpdateExpense;
    }

    public void Handle(AddExpenseInfo command) {
      addUpdateExpense.Handle(command.GetAddUpdateInfo());

      expenseDb.SaveChanges();
    }
  }

  public class AddExpenseInfo {
    public string name { get; set; }
    public DateTime billedDate { get; set; }
    public DateTime? effectiveDate { get; set; }

    public List<ExpenseLine> expenseLines { get; set; }

    public AddExpenseInfo() { }

    public AddUpdateExpenseInfo GetAddUpdateInfo() {
      return new AddUpdateExpenseInfo() {
        // When adding use an ID of 0 and always include
        // the expense lines.
        expenseId = 0,
        expenseLinesIncluded = true,
        name = name,
        billedDate = billedDate,
        effectiveDate = effectiveDate,
        expenseLines = expenseLines
      };
    }
  }
}
