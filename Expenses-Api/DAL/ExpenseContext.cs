using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.Models;
using ExpensesApi.DAL.Maps;

namespace ExpensesApi.DAL {
  public class ExpenseContext : DbContext {
    public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseLine> ExpenseLines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
      modelBuilder.AddConfiguration(new ExpenseMap());
      modelBuilder.AddConfiguration(new ExpenseLineMap());
    }
  }
}
