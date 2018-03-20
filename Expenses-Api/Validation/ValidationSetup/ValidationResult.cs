using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Validation {
  /// <summary>
  /// Key - Message object for listing validation
  /// errors so that they can be displayed to users
  /// </summary>
  public class ValidationResult {
    /// <summary>
    /// ValidationResult constructor.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="message"></param>
    public ValidationResult(string key, string message) {
      this.Key = key;
      this.Message = message;
    }
    /// <summary>
    /// Key for the result, normally this is the 
    /// name of the field which hit an error.
    /// </summary>
    public string Key { get; private set; }
    /// <summary>
    /// The message to be displayed to the user.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Override the Equals function, this ensures
    /// that behaviour in lists and dictionaries is
    /// correct.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Override the GetHashCode function, this ensures
    /// that behaviour in lists and dictionaries is
    /// correct.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() {
      int hash = 13;
      hash = (hash * 7) + (Key != null ? Key.GetHashCode() : 0);
      hash = (hash * 7) + (Message != null ? Message.GetHashCode() : 0);
      return hash;
    }
  }
}
