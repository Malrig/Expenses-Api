using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.Expenses {
  /// <summary>
  /// Command to update an existing expense item using information
  /// from the UpdateExpenseInfo class.
  /// Note that this doesn't handle any database functionality
  /// and instead just passes the result to the AddUpdateExpense
  /// command.
  /// </summary>
  public class UpdateExpense : ICommandHandler<UpdateExpenseInfo> {
    private ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense;
    
    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="addUpdateExpense"></param>
    public UpdateExpense(ICommandHandler<AddUpdateExpenseInfo> addUpdateExpense) {
      this.addUpdateExpense = addUpdateExpense;
    }

    /// <summary>
    /// Handle the command, currently this only requires a 
    /// call to the AddUpdateExpense command.
    /// </summary>
    /// <param name="command"></param>
    public void Handle(UpdateExpenseInfo command) {
      addUpdateExpense.Handle(command.GetAddUpdateInfo());
    }
  }

  /// <summary>
  /// Class containing all the information required to update
  /// an expense.
  /// </summary>
  public class UpdateExpenseInfo {
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
    public List<ExpenseLine> expenseLines { get; set; }

    /// <summary>
    /// Whether or not the expense lines for the expense
    /// should be updated.
    /// </summary>
    public bool expenseLinesIncluded { get; set; } = false;

    /// <summary>
    /// Default constructor required for EntityFramework
    /// </summary>
    public UpdateExpenseInfo() { }

    /// <summary>
    /// Function which returns the info to pass to the 
    /// AddUpdateExpense command.
    /// </summary>
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
