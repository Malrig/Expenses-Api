using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpensesApi.Identity.Models {
  public class TokenAuthOptions {
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public SigningCredentials SigningCredentials { get; set; }
  }
}
