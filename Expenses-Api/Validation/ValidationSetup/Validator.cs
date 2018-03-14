using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {
  public interface IValidator {
    IEnumerable<ValidationResult> Validate(object entity);
  }

  public abstract class Validator<T> : IValidator {
    IEnumerable<ValidationResult> IValidator.Validate(object entity) {
      if (entity == null) {
        throw new ArgumentNullException("entity");
      }

      return this.Validate((T)entity);
    }

    protected abstract IEnumerable<ValidationResult> Validate(T entity);
  }
}