using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {
  /// <summary>
  /// Interface which exposes a method which validates
  /// a given command or entity.
  /// </summary>
  public interface IValidator {
    /// <summary>
    /// Function which actually performs the validation
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    IEnumerable<ValidationResult> Validate(object entity);
  }

  /// <summary>
  /// Abstract base class which forms the validator for 
  /// a particular object "T".
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Validator<T> : IValidator {
    IEnumerable<ValidationResult> IValidator.Validate(object entity) {
      if (entity == null) {
        throw new ArgumentNullException("entity");
      }

      return this.Validate((T)entity);
    }

    /// <summary>
    /// Function which actually performs the validation,
    /// this must be overridden in any child classes.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected abstract IEnumerable<ValidationResult> Validate(T entity);
  }
}