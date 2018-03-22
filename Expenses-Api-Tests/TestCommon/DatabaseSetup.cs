using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.DAL;

namespace ExpensesApiTests.TestCommon {
  public static class DatabaseSetup {
    public static ExpenseContext CreateExpenseContext() {
      return CreateExpenseContext(Guid.NewGuid().ToString());
    }

    public static ExpenseContext CreateExpenseContext(string contextName) {
      var options = new DbContextOptionsBuilder<ExpenseContext>()
        .UseInMemoryDatabase(databaseName: contextName)
        .Options;

      return new ExpenseContext(options);
    }
  }
}
