using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.DAL;
using ExpensesApi.Models;
using ExpensesApi.ViewModels;

namespace ExpensesApi.Services.Expenses {
  /// <summary>
  /// Query to return a detailed record of
  /// an expense item
  /// </summary>
  public class GetExpenseDetail : IQueryHandler<FindExpenseById, ExpenseDetail> {
    private ExpenseContext expenseDb;

    /// <summary>
    /// Constructor pulls in all the required services
    /// </summary>
    /// <param name="expenseDb"></param>
    public GetExpenseDetail(ExpenseContext expenseDb) {
      this.expenseDb = expenseDb;
    }

    /// <summary>
    /// Actually perform the query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
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

  /// <summary>
  /// The class which contains all the information 
  /// required to run the above query
  /// </summary>
  public class FindExpenseById : IQuery<ExpenseDetail> {
    /// <summary>
    /// ID of the expense item
    /// </summary>
    public int expenseId;

    /// <summary>
    /// Constructor which sets the expense ID
    /// </summary>
    /// <param name="expenseId"></param>
    public FindExpenseById(int expenseId) {
      this.expenseId = expenseId;
    }
  }
}
