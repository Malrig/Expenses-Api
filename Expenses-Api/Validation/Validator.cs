using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {

  public interface IValidator {
    void Validate(object entity);
    void ValidateAll(IList<object> entities);
  }

  public abstract class Validator<T> : IValidator {
    void IValidator.Validate(object entity) {
      if (entity == null) {
        throw new ArgumentNullException("entity");
      }

      var validationResults = this.PerformValidation((T)entity).ToArray();

      if (validationResults.Length > 0) {
        throw new ValidationException(validationResults);
      }
    }

    void IValidator.ValidateAll(IList<object> entities) {
      if (entities == null) {
        throw new ArgumentNullException("entity");
      }

      var validationResults = (from entity in entities.Cast<T>()
                               let validator = this
                               from result in validator.PerformValidation(entity)
                               select result).ToArray();
      
      if (validationResults.Length > 0) {
        throw new ValidationException(validationResults);
      }
    }

    protected abstract IEnumerable<ValidationResult> PerformValidation(T entity);
  }
}