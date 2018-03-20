using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore {
  /// <summary>
  /// Abstract class which provides the basis for mapping
  /// Models to database tables/columns.
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  public abstract class EntityTypeConfiguration<TEntity>
      where TEntity : class {
    /// <summary>
    /// Function which maps models to their properties in 
    /// the database.
    /// </summary>
    /// <param name="builder"></param>
    public abstract void Map(EntityTypeBuilder<TEntity> builder);
  }

  /// <summary>
  /// Class which extends the ModelBuilder so that it can 
  /// consume the EntityTypeConfiguration class.
  /// </summary>
  public static class ModelBuilderExtensions {
    /// <summary>
    /// Function which adds the configuration to the system.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="modelBuilder"></param>
    /// <param name="configuration"></param>
    public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity> configuration)
        where TEntity : class {
      configuration.Map(modelBuilder.Entity<TEntity>());
    }
  }
}