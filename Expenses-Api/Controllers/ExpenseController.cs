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
      expenseService = new ExpenseService(this.ModelState, db);
      this.ModelState.AddModelError("Name", "Name is required.");
    }

    // GET api/expenses
    [HttpGet]
    public IActionResult Get() {
      return Ok(expenseService.ListExpenses());
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int? id) {
      if (id == null) {
        return BadRequest("You must supply an id when querying an individual expense.");
      }

      var toReturn = expenseService.GetExpense((int)id);

      if (toReturn != null) {
        return Ok(toReturn);
      }
      else {
        return NotFound("No expense found with ID " + id);
      }

    }

    // POST api/values
    [HttpPost]
    public IActionResult Post(Expense expenseToAdd) {
      try {
        expenseService.CreateExpense(expenseToAdd);
      }
      catch (ValidationException ex) {
        MvcValidationExtension.AddModelErrors(this.ModelState, ex);
        return BadRequest(this.ModelState);
      }
      
      return Ok("Expense created successfully.");
    }

    //// PUT api/values/5
    //[HttpPut("{id}")]
    //public IActionResult Put(int? id, Expense expenseToProcess) {
    //  List<string> errorMessages = new List<string>();

    //  if (id == null) {
    //    return BadRequest("You must supply an id when updating an expense.");
    //  }

    //  if (expenseToProcess == null) {
    //    return BadRequest("You must supply the updated expense.");
    //  }

    //  Expense existingExpense = db.Expenses.Where(e => e.expenseId == id)
    //                                       .Include(e => e.expenseLines)
    //                                       .SingleOrDefault();

    //  // Perform various checks to make sure it is a valid expense
    //  if ((expenseToProcess.expenseLines == null) ||
    //      (expenseToProcess.expenseLines.Count() == 0)) {
    //    errorMessages.Add("No expense lines present on the expense. ");
    //  }

    //  // Update the expense
    //  db.Entry(existingExpense).CurrentValues.SetValues(expenseToProcess);

    //  // Delete any expense lines which are no longer required
    //  foreach (ExpenseLine existingLine in existingExpense.expenseLines) {
    //    if (!expenseToProcess.expenseLines.Any(el => el.expenseLineId == existingLine.expenseLineId)) {
    //      db.ExpenseLines.Remove(existingLine);
    //    }
    //  }

    //  if (errorMessages.Count == 0) {
    //    // Update and add any expense lines
    //    foreach (ExpenseLine lineToProcess in expenseToProcess.expenseLines) {
    //      ExpenseLine existingLine =
    //        existingExpense.expenseLines
    //                       .Where(el => ((el.expenseLineId == lineToProcess.expenseLineId) &&
    //                                     (lineToProcess.expenseLineId != 0)))
    //                       .SingleOrDefault();

    //      if (existingLine != null) {
    //        // Schedule portion already exists so update it
    //        db.Entry(existingLine).CurrentValues.SetValues(lineToProcess);
    //      }
    //      else {
    //        // Insert new child
    //        ExpenseLine newLine = new ExpenseLine {
    //          expenseId = existingExpense.expenseId,
    //          name = lineToProcess.name,
    //          amount = lineToProcess.amount
    //        };

    //        existingExpense.expenseLines.Add(newLine);
    //      }
    //    }

    //    try {
    //      db.SaveChanges();
    //    }
    //    catch (Exception ex) {
    //      // TODO Don't return the exception but instead log it and add a user friendly message.
    //      //errorMessages.Add("Failed to update the database, please try again.");
    //      return BadRequest(ex);
    //    }
    //  }

    //  if (errorMessages.Count > 0) {
    //    return BadRequest(errorMessages);
    //  }
    //  else {
    //    return Ok("Expense successfully updated.");
    //  }
    //}

    ////// DELETE api/values/5
    ////[HttpDelete("{id}")]
    ////public void Delete(int id) {
    ////}
  }
}
