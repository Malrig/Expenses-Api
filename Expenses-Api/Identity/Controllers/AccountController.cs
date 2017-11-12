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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountRegisterLogin model) {
      if (!ModelState.IsValid) {
        return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(modelError => modelError.ErrorMessage).ToList());
      }

      var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
      var result = await userManager.CreateAsync(user, model.Password);

      if (!result.Succeeded) {
        return BadRequest(result.Errors.Select(x => x.Description).ToList());
      }

      await signInManager.SignInAsync(user, false);

      return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountRegisterLogin model) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

      if (!result.Succeeded) {
        return BadRequest();
      }

      return Ok();
    }
  }
}
