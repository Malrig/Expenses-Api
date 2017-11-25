using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ExpensesApi.Identity.DAL;
using ExpensesApi.Identity.Models;

namespace ExpensesApi.Identity.Controllers {
  [Route("api/[controller]")]
  public class AccountController : Controller {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    //private readonly IPasswordHasher<ApplicationUser> passwordHasher;

    public AccountController(UserManager<ApplicationUser> userManager, 
                             SignInManager<ApplicationUser> signInManager,
                             IPasswordHasher<ApplicationUser> passwordHasher) {
      this.userManager = userManager;
      this.signInManager = signInManager;
      //this.passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Creates an account.
    /// </summary>
    /// <remarks> 
    /// Sample request:
    ///
    ///     {
    ///         "email": "test@dummymail.com",
    ///         "password": "P4ssw0rd!"
    ///     }
    ///
    /// </remarks>
    /// <param name="newAccount">The new account to create.</param>
    /// <returns></returns>
    /// <response code="200">Returns a successful message.</response>
    /// <response code="400">If the account fails validation checks.</response>  
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountRegisterLogin newAccount) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(modelError => modelError.ErrorMessage).ToList());
      }

      var user = new ApplicationUser { UserName = newAccount.email, Email = newAccount.email };
      var result = await userManager.CreateAsync(user, newAccount.password);

      if (!result.Succeeded) {
        return BadRequest(result.Errors.Select(x => x.Description).ToList());
      }

      await signInManager.SignInAsync(user, false);

      return Ok();
    }

    /// <summary>
    /// Logs into an existing account.
    /// </summary>
    /// <remarks> 
    /// Sample request:
    ///
    ///     {
    ///         "email": "test@dummymail.com",
    ///         "password": "P4ssw0rd!"
    ///     }
    ///
    /// </remarks>
    /// <param name="loginAccount">The details of the account to log in.</param>
    /// <returns></returns>
    /// <response code="200">Returns a successful message, along with a session cookie.</response>
    /// <response code="400">If the account fails validation checks.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountRegisterLogin loginAccount) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      var result = await signInManager.PasswordSignInAsync(loginAccount.email, loginAccount.password, isPersistent: false, lockoutOnFailure: false);

      if (!result.Succeeded) {
        return BadRequest();
      }

      return Ok();
    }
  }
}
