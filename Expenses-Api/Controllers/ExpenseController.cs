﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

using ExpensesApi.Helpers;
using ExpensesApi.Models;
using ExpensesApi.ViewModels;
using ExpensesApi.Services;
using ExpensesApi.Services.Expenses;

namespace ExpensesApi.Controllers {
  /// <summary>
  /// Handles all requests concerning expenses
  /// </summary>
  [Authorize]
  [Route("api/[controller]")]
  public class ExpenseController : Controller {
    private IQueryHandler<FindAllExpenses, ExpensesOverview> getExpensesOverview { get; }
    private IQueryHandler<FindExpenseById, ExpenseDetail> getExpenseDetail { get; }
    private ICommandHandler<AddExpenseInfo> addExpense { get; }
    private ICommandHandler<UpdateExpenseInfo> updateExpense { get; }
    private ICommandHandler<DeleteExpenseInfo> deleteExpense { get; }

    /// <summary>
    /// Constructor for the expense controller
    /// </summary>
    /// <param name="getExpensesOverview"></param>
    /// <param name="getExpenseDetail"></param>
    /// <param name="addExpense"></param>
    /// <param name="updateExpense"></param>
    /// <param name="deleteExpense"></param>
    public ExpenseController(IQueryHandler<FindAllExpenses, ExpensesOverview> getExpensesOverview,
                             IQueryHandler<FindExpenseById, ExpenseDetail> getExpenseDetail,
                             ICommandHandler<AddExpenseInfo> addExpense,
                             ICommandHandler<UpdateExpenseInfo> updateExpense,
                             ICommandHandler<DeleteExpenseInfo> deleteExpense) {
      this.getExpensesOverview = getExpensesOverview;
      this.getExpenseDetail = getExpenseDetail;
      this.addExpense = addExpense;
      this.updateExpense = updateExpense;
      this.deleteExpense = deleteExpense;
    }
    
    /// <summary>
    /// Get all expenses
    /// </summary>
    /// <remarks>
    /// Get a list of all expenses
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Returns a successful message</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Expense>), 200)]
    public IActionResult Get() {
      try {
        return Ok(getExpensesOverview.Handle(new FindAllExpenses()));
      }
      catch (Exception e) {
        // TODO - Log exception
        return new ExceptionActionResult(e);
      }
    }

    /// <summary>
    /// Get a single expenses
    /// </summary>
    /// <remarks>
    /// Get an individual expense
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Returns a successful message</response>
    /// <response code="404">If the expense item does not exist</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ExpenseDetail), 200)]
    [ProducesResponseType(typeof(String), 404)]
    public IActionResult Get(int id) {
      ExpenseDetail expenseToReturn;

      try {
        expenseToReturn = getExpenseDetail.Handle(new FindExpenseById(id));
      }
      catch (Exception e) {
        // TODO - Log exception
        return new ExceptionActionResult(e);
      }

      return Ok(expenseToReturn);
    }

    /// <summary>
    /// Creates an expense item.
    /// </summary>
    /// <remarks>
    /// Creates an expense item including it's expense lines.
    /// 
    /// Sample request:
    ///
    ///     {
    ///         "name": "New Title",
    ///         "billedDate": "2017-02-01T00:00:00",
    ///         "effectiveDate": null,
    ///         "expenseLines": [
    ///             {
    ///                 "name": "New Entry 1",
    ///                 "amount": 100
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <param name="expenseToProcess">The expense to create</param>
    /// <returns></returns>
    /// <response code="200">Returns a successful message</response>
    /// <response code="400">If the item fails validation</response>    
    [HttpPost]
    [ProducesResponseType(typeof(String), 200)]
    [ProducesResponseType(typeof(ModelStateDictionary), 400)]
    public IActionResult Create([FromBody]AddExpenseInfo expenseToProcess) {
      try {
        addExpense.Handle(expenseToProcess);
      }
      catch (Exception e) {
        // TODO - Log exception
        return new ExceptionActionResult(e);
      }

      return Ok("Expense created successfully.");
    }


    /// <summary>
    /// Updates an expense item.
    /// </summary>
    /// <remarks>
    /// Updates an expense and adds/updates/deletes it's expense lines.
    /// 
    /// Sample request:
    ///
    ///     {
    ///         "name": "New Title",
    ///         "billedDate": "2017-02-01T00:00:00",
    ///         "effectiveDate": null,
    ///         "expenseLinesIncluded": "True",
    ///         "expenseLines": [
    ///             {
    ///                 "name": "New Entry 1",
    ///                 "amount": 100
    ///             }
    ///         ]
    ///     }
    ///
    /// </remarks>
    /// <param name="id">The ID of the expense to update</param>
    /// <param name="expenseToProcess">The values to change to</param>
    /// <returns></returns>
    /// <response code="200">Returns a successful message</response>
    /// <response code="400">If the item fails validation</response> 
    /// <response code="404">If the expense item does not exist</response>
    [HttpPut("{id}")]
    [HttpPost("{id}")]
    [ProducesResponseType(typeof(String), 200)]
    [ProducesResponseType(typeof(ModelStateDictionary), 400)]
    [ProducesResponseType(typeof(String), 404)]
    public IActionResult Update(int id, [FromBody]UpdateExpenseInfo expenseToProcess) {
      if ((expenseToProcess.expenseId != 0) &&
          (expenseToProcess.expenseId != id)) {
        // TODO - Throw an error as the IDs do not match.
      }

      expenseToProcess.expenseId = id;

      try {
        updateExpense.Handle(expenseToProcess);
      }
      catch (Exception e) {
        // TODO - Log exception
        return new ExceptionActionResult(e);
      }

      return Ok("Expense updated successfully.");
    }

    /// <summary>
    /// Delete an expenses
    /// </summary>
    /// <remarks>
    /// Delete an individual expense and it's expense lines
    /// </remarks>
    /// <returns></returns>
    /// <response code="204">Delete successful returns no content</response>
    /// <response code="404">If the expense item does not exist</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(string), 404)]
    public IActionResult Delete(int id) {
      try {
        deleteExpense.Handle(new DeleteExpenseInfo(id));
      }
      catch (Exception e) {
        // TODO - Log exception
        return new ExceptionActionResult(e);
      }

      return NoContent();
    }
  }
}
