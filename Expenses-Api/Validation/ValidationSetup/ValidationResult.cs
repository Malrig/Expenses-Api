using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {
  public class ValidationResult {
    public ValidationResult(string key, string message) {
      this.Key = key;
      this.Message = message;
    }
    public string Key { get; private set; }
    public string Message { get; private set; }

    public override bool Equals(object obj) {
      if (!(obj is ValidationResult)) {
        return false;
      }
      ValidationResult other = (ValidationResult)obj;

      if ((!other.Key.Equals(this.Key)) ||
          (!other.Message.Equals(this.Message))) {
        return false;
      }

      return true;
    }

    public override int GetHashCode() {
      int hash = 13;
      hash = (hash * 7) + (Key != null ? Key.GetHashCode() : 0);
      hash = (hash * 7) + (Message != null ? Message.GetHashCode() : 0);
      return hash;
    }
  }
}
