using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.DAL;
using ExpensesApi.Models;

namespace ExpensesApi.Services.ExpenseLines {
  public class AddUpdateExpenseLine : ICommandHandler<AddUpdateExpenseLineInfo> {
    private ExpenseContext expenseDb;

    public AddUpdateExpenseLine(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

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

  public class AddUpdateExpenseLineInfo {
    public int expenseLineId { get; set; }
    public string name { get; set; }
    public decimal amount { get; set; }
    public int expenseId { get; set; }

    public AddUpdateExpenseLineInfo() { }
    public AddUpdateExpenseLineInfo(ExpenseLine expenseLine) {
      this.expenseLineId = expenseLine.expenseLineId;
      this.name = expenseLine.name;
      this.amount = expenseLine.amount;
      this.expenseId = expenseLine.expenseId;
    }

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
