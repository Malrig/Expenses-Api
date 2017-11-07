using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ExpensesApi.Identity;

namespace ExpensesApi.Identity.DAL {
  public class IdentityContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int> {
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

    //public DbSet<Expense> Expenses { get; set; }
    //public DbSet<ExpenseLine> ExpenseLines { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder){
    //  modelBuilder.AddConfiguration(new ExpenseMap());
    //  modelBuilder.AddConfiguration(new ExpenseLineMap());
    //}
  }

  //public class ApplicationDbContextFactory : IDbContextFactory<IdentityContext> {
  //  public IdentityContext Create(DbContextFactoryOptions options) {
  //    var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>().UseSqlServer("Server=192.168.1.72;Database=IdentityTest;Integrated Security=False;MultipleActiveResultSets=True;User ID=sa;Password=i1yuPtFOrIUjC7S4;");
  //    // TODO work out how to do migrations properly
  //    return new IdentityContext(optionsBuilder.Options);
  //  }
  //}
}
