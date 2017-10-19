using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExpensesApi.Models;

namespace ExpensesApi.ViewModels {
  public class SimpleExpenseList {
    List<SimpleExpense> expenses;

    public SimpleExpenseList(IEnumerable<Expense> originalExpenses){
      this.expenses = new List<SimpleExpense>();

      foreach(Expense originalExpense in originalExpenses){
        this.expenses.Add(new SimpleExpense(originalExpense));
      }
    }

  }
}
