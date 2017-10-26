using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {
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
}
