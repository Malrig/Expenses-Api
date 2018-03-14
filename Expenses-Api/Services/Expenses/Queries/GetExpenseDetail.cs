using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.ViewModels;

namespace ExpensesApi.Services.Expenses
{
  public class GetExpenseDetail : IQueryHandler<FindExpenseById, ExpenseDetail> {
    private ExpenseContext expenseDb;

    public GetExpenseDetail(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    public ExpenseDetail Handle(FindExpenseById query) {
      Expense expenseToReturn = expenseDb.Expenses.Where(e => e.expenseId == query.expenseId)
                                                  .Include(e => e.expenseLines)
                                                  .SingleOrDefault();

      if (expenseToReturn == null) {
        throw new KeyNotFoundException("No expense with ID: " + query.expenseId);
      }

      return new ExpenseDetail(expenseToReturn);
    }
  }

  public class FindExpenseById : IQuery<ExpenseDetail> {
    public int expenseId;

    public FindExpenseById(int expenseId) {
      this.expenseId = expenseId;
    }
  }
}
