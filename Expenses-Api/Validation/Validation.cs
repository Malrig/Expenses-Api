using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {

  public interface IValidator {
    void Validate(object entity);
  }

  public class ValidationResult {
    public ValidationResult(string key, string message) {
      this.key = key;
      this.message = message;
    }
    public string key { get; private set; }
    public string message { get; private set; }

    public override bool Equals(object obj) {
      if (!(obj is ValidationResult)) {
        return false;
      }
      ValidationResult other = (ValidationResult)obj;

      if ((!other.key.Equals(this.key)) ||
          (!other.message.Equals(this.message))) {
        return false;
      }

      return true;
    }

    public override int GetHashCode() {
      int hash = 13;
      hash = (hash * 7) + (key != null ? key.GetHashCode() : 0);
      hash = (hash * 7) + (message != null ? message.GetHashCode() : 0);
      return hash;
    }
  }

  public class ValidationException : Exception {
    public ValidationException(IEnumerable<ValidationResult> r) : base(GetFirstErrorMessage(r)) {
      this.ValidationErrors = new ReadOnlyCollection<ValidationResult>(r.ToArray());
    }

    public ReadOnlyCollection<ValidationResult> ValidationErrors { get; private set; }

    private static string GetFirstErrorMessage(
        IEnumerable<ValidationResult> errors) {
      return errors.First().message;
    }
  }

  public abstract class Validator<T> : IValidator {
    void IValidator.Validate(object entity) {
      if (entity == null) {
        throw new ArgumentNullException("entity");
      }

      this.Validate((T)entity);
    }

    protected void Validate(T entity) {
      var validationResults = this.PerformValidation(entity).ToArray();

      if (validationResults.Length > 0) {
        throw new ValidationException(validationResults);
      }
    }

    protected abstract IEnumerable<ValidationResult> PerformValidation(T entity);
  }
}
