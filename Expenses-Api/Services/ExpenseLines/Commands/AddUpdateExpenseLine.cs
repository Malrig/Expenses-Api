using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.ExpenseLines {
  /// <summary>
  /// Command to add or update a given expense line using the information
  /// provided in the AddUpdateExpenseLineInfo class.
  /// </summary>
  public class AddUpdateExpenseLine : ICommandHandler<AddUpdateExpenseLineInfo> {
    private ExpenseContext expenseDb;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expenseDb"></param>
    public AddUpdateExpenseLine(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    /// <summary>
    /// This handles the command making all the required changes
    /// to the database.
    /// </summary>
    /// <param name="command"></param>
    public void Handle(AddUpdateExpenseLineInfo command) {
      ExpenseLine existingLine = expenseDb.ExpenseLines.Where(e => e.expenseLineId == command.expenseLineId)
                                                       .SingleOrDefault();
      
      if (existingLine == null) {
        expenseDb.ExpenseLines.Add(command.GetExpenseLine());
      }
      else {
        expenseDb.Entry(existingLine).CurrentValues.SetValues(command);
      }

      expenseDb.SaveChanges();
    }
  }

  /// <summary>
  /// Class containing all the information required to add
  /// or update an expense line.
  /// </summary>
  public class AddUpdateExpenseLineInfo {
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

    /// <summary>
    /// Default constructor required for EntityFramework
    /// </summary>
    public AddUpdateExpenseLineInfo() { }
    /// <summary>
    /// Constructor from an expense line
    /// </summary>
    /// <param name="expenseLine"></param>
    // TODO remove this constructor
    public AddUpdateExpenseLineInfo(ExpenseLine expenseLine) {
      this.expenseLineId = expenseLine.expenseLineId;
      this.name = expenseLine.name;
      this.amount = expenseLine.amount;
      this.expenseId = expenseLine.expenseId;
    }

    /// <summary>
    /// Function which returns the expense line item, used for
    /// adding it to the database.
    /// </summary>
    public ExpenseLine GetExpenseLine() {
      return new ExpenseLine() {
        expenseLineId = expenseLineId,
        name = name,
        amount = amount,
        expenseId = expenseId
      };
    }
  }
}
