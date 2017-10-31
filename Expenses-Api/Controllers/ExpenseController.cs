using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using ExpensesApi.Models;
using ExpensesApi.ViewModels;
using ExpensesApi.DAL;
using ExpensesApi.Services;
using ExpensesApi.Validation;

namespace ExpensesApi.Controllers {

  public static class MvcValidationExtension {
    public static void AddModelErrors(this ModelStateDictionary state, ValidationException exception) {
      foreach (var error in exception.ValidationErrors) {
        state.AddModelError(error.key, error.message);
      }
    }
  }

  [Route("api/[controller]")]
  public class ExpenseController : Controller {
    private IExpenseService expenseService;

    public ExpenseController(ExpenseContext db) {
      expenseService = new ExpenseService(db);
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
    public IActionResult Get() {
      try {
        return Ok(expenseService.ListExpenses());
      }
      catch (Exception e) {
        // TODO - Log exception
        return StatusCode(500, e);
      }
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int id) {
      Expense expenseToReturn;

      try {
        expenseToReturn = expenseService.GetExpense(id);
      }
      catch (KeyNotFoundException noKeyEx) {
        return NotFound(noKeyEx.Message);
      }
      catch (Exception e) {
        // TODO - Log exception
        return StatusCode(500, e);
      }

      return Ok(expenseToReturn);
    }

    /// <summary>
    /// Creates an expense item.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
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
    /// <param name="expenseToProcess"></param>
    /// <returns>A 200 response</returns>
    /// <response code="200">Returns a successful message</response>
    /// <response code="400">If the item fails validation</response>    
    [HttpPost]
    [ProducesResponseType(typeof(Expense), 201)]
    [ProducesResponseType(typeof(Expense), 400)]
    public IActionResult Create([FromBody]Expense expenseToProcess) {
      try {
        expenseService.CreateExpense(expenseToProcess);
      }
      catch (ValidationException ex) {
        MvcValidationExtension.AddModelErrors(this.ModelState, ex);
        return BadRequest(this.ModelState);
      }
      catch (Exception e) {
        // TODO - Log exception
        return StatusCode(500, e);
      }

      return Ok("Expense created successfully.");
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    [HttpPost("{id}")]
    public IActionResult Update(int id, [FromBody]Expense expenseToProcess) {
      try {
        expenseService.UpdateExpense(id, expenseToProcess);
      }
      catch (ValidationException valEx) {
        MvcValidationExtension.AddModelErrors(this.ModelState, valEx);
        return BadRequest(this.ModelState);
      }
      catch (KeyNotFoundException noKeyEx) {
        return NotFound(noKeyEx.Message);
      }
      catch (Exception e) {
        // TODO - Log exception
        return StatusCode(500, e);
      }

      return Ok("Expense updated successfully.");
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
      try {
        expenseService.DeleteExpense(id);
      }
      catch (KeyNotFoundException noKeyEx) {
        return NotFound(noKeyEx.Message);
      }
      catch (Exception e) {
        // TODO - Log exception
        return StatusCode(500, e.Message);
      }

      return NoContent();
    }
  }
}
