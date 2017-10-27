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

    // GET api/expenses
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
    public IActionResult Get(int? id) {
      if (id == null) {
        return BadRequest("You must supply an id when querying an individual expense.");
      }

      try {
        var toReturn = expenseService.GetExpense((int)id);

        if (toReturn != null) {
          return Ok(toReturn);
        }
        else {
          return NotFound("No expense found with ID " + id);
        }
      }
      catch (Exception e) {
        // TODO - Log exception
        return StatusCode(500, e);
      }
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody]Expense expenseToProcess) {
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
    public IActionResult Put(int id, [FromBody]Expense expenseToProcess) {
      try {
        expenseService.UpdateExpense(id, expenseToProcess);
      }
      catch (ValidationException valEx) {
        MvcValidationExtension.AddModelErrors(this.ModelState, valEx);
        return BadRequest(this.ModelState);
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
        return StatusCode(500, e);
      }

      return NoContent();
    }
  }
}
