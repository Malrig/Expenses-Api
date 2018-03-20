using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.Expenses {
  /// <summary>
  /// Command to add a new expense item using information
  /// from the AddExpenseInfo class.
  /// Note that this doesn't handle any database functionality
  /// and instead just passes the result to the AddUpdateExpense
  /// command.
  /// </summary>
  public class AddExpense : ICommandHandler<AddExpenseInfo> {
    private ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="addUpdateExpense"></param>
    public AddExpense(ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense) {
      this.addUpdateExpense = addUpdateExpense;
    }

    /// <summary>
    /// Handle the command, currently this only requires a 
    /// call to the AddUpdateExpense command.
    /// </summary>
    /// <param name="command"></param>
    public void Handle(AddExpenseInfo command) {
      addUpdateExpense.Handle(command.GetAddUpdateInfo());
    }
  }

  /// <summary>
  /// Class containing all the information required to add
  /// an expense.
  /// </summary>
  public class AddExpenseInfo {
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
    public List<ExpenseLine> expenseLines { get; set; }

    /// <summary>
    /// Default constructor required for EntityFramework
    /// </summary>
    public AddExpenseInfo() { }

    /// <summary>
    /// Function which returns the info to pass to the 
    /// AddUpdateExpense command.
    /// </summary>
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
