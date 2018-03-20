using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ExpensesApi {
  /// <summary>
  /// Class which controlls how the program is started
  /// </summary>
  public class Program {
    /// <summary>
    /// Main function
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args) {
      BuildWebHost(args).Run();
    }

    /// <summary>
    /// Function which sets up the web hosting for the API
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IWebHost BuildWebHost(string[] args) {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("hosting.json", optional: true)
          .AddCommandLine(args)
          .Build();

      return WebHost.CreateDefaultBuilder(args)
          .UseStartup<Startup>()
          .UseConfiguration(config)
          .Build();
    }
  }
}
