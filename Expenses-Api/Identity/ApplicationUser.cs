﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExpensesApi.Identity {
  public class ApplicationUser : IdentityUser<int> {
  }
}
