﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT.Controllers {
  [Route("api/[controller]")]
  public class TokenController : Controller {
    private IConfiguration configuration;

    public TokenController(IConfiguration config) {
      configuration = config;
    }

    /// <summary>
    /// Logs into an existing account.
    /// </summary>
    /// <remarks> 
    /// Sample request:
    ///
    ///     {
    ///         "username": "TestAdmin",
    ///         "password": "TestPassword"
    ///     }
    ///
    /// </remarks>
    /// <param name="loginAccount">The details of the account to log in.</param>
    /// <returns></returns>
    /// <response code="200">Returns a successful message, along with a JWT token.</response>
    /// <response code="400">If the account fails validation checks.</response>
    [AllowAnonymous]
    [HttpPost]
    public IActionResult CreateToken([FromBody]LoginModel loginAccount) {
      IActionResult response = Unauthorized();

      if (Authenticate(loginAccount)) {
        var tokenString = BuildToken(loginAccount);
        return Ok(new { token = tokenString });
      }

      return BadRequest("Username or password is incorrect");
    }

    private string BuildToken(LoginModel user) {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        configuration["Jwt:Issuer"],
        configuration["Jwt:Issuer"],
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool Authenticate(LoginModel login) {
      if (((login.Username == configuration["Jwt:AdminAccount:Username"]) && 
           (login.Password == configuration["Jwt:AdminAccount:Password"])) ||
          (Convert.ToBoolean(configuration["Jwt:AllowAnonymousLogin"]))) {
        return true;
      }
      return false;
    }

    public class LoginModel {
      public string Username { get; set; }
      public string Password { get; set; }
    }
  }
}