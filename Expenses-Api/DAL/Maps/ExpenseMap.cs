using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ExpensesApi.Models;

namespace ExpensesApi.DAL.Maps {
  /// <summary>
  /// Class which maps the Expense models properties
  /// to database tables and columns.
  /// </summary>
  public class ExpenseMap : EntityTypeConfiguration<Expense> {
    /// <summary>
    /// Function which maps models to their properties in 
    /// the database.
    /// </summary>
    /// <param name="builder"></param>
    public override void Map(EntityTypeBuilder<Expense> builder) {
      builder.ToTable("expense", "dbo");
      builder.HasKey(b => b.expenseId);
      builder.Property(b => b.expenseId)
             .HasColumnName("expenseId");
      builder.Property(b => b.name)
             .HasColumnName("e_name");
      builder.Property(b => b.billedDate)
             .HasColumnName("e_billedDate");
      builder.Property(b => b.effectiveDate)
             .HasColumnName("e_effectiveDate");
    }
  }
}
