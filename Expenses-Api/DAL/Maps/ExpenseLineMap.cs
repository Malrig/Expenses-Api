using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ExpensesApi.Models;

namespace ExpensesApi.DAL.Maps {
  /// <summary>
  /// Class which maps the ExpenseLine models properties
  /// to database tables and columns.
  /// </summary>
  public class ExpenseLineMap : EntityTypeConfiguration<ExpenseLine> {
    /// <summary>
    /// Function which maps models to their properties in 
    /// the database.
    /// </summary>
    /// <param name="builder"></param>
    public override void Map(EntityTypeBuilder<ExpenseLine> builder) {
      builder.ToTable("expenseLine", "dbo");
      builder.HasKey(b => b.expenseLineId);
      builder.Property(b => b.expenseLineId)
             .HasColumnName("expenseLineId");
      builder.Property(b => b.name)
             .HasColumnName("el_name");
      builder.Property(b => b.amount)
             .HasColumnName("el_amount");
      builder.Property(b => b.expenseId)
             .HasColumnName("el_expenseId");
      builder.HasOne<Expense>(b => b.expense)
             .WithMany(e => e.expenseLines)
             .HasForeignKey(b => b.expenseId);
    }
  }
}
