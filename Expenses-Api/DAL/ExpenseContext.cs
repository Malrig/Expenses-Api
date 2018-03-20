using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.Models;
using ExpensesApi.DAL.Maps;

namespace ExpensesApi.DAL {
  /// <summary>
  /// Class representing the Expenses database,
  /// contains sets for all its different entities.
  /// </summary>
  public class ExpenseContext : DbContext {
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options"></param>
    public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options) { }

    /// <summary>
    /// DbSet containing all expenses
    /// </summary>
    public DbSet<Expense> Expenses { get; set; }
    /// <summary>
    /// DbSet containing all expense lines
    /// </summary>
    public DbSet<ExpenseLine> ExpenseLines { get; set; }

    /// <summary>
    /// Function called when models are generated, adds all
    /// the configuration for the different database models.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder){
      modelBuilder.AddConfiguration(new ExpenseMap());
      modelBuilder.AddConfiguration(new ExpenseLineMap());
    }
  }
}
