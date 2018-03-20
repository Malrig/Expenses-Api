using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Services {
  /// <summary>
  /// Interface for the query classes, they
  /// expose a single function Handle which 
  /// performs the query 
  /// </summary>
  /// <typeparam name="TQuery"></typeparam>
  /// <typeparam name="TResult"></typeparam>
  public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> {
    /// <summary>
    /// Function which performs the actual query,
    /// all information for the query should be included
    /// in the TQuery class.
    /// The result is returned in the form of the TResult class.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    TResult Handle(TQuery query);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="TResult"></typeparam>
  public interface IQuery<TResult> {
  }
}
