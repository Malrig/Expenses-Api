using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using ExpensesApi.DAL;

namespace ExpensesApi {
  public class Startup {
    public Startup(IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      // Add database contexts
      //var subscriptionContext = "Server=KNOX;Database=msghardwaretest;Integrated Security=False;MultipleActiveResultSets=True;User ID=msghardware_user;Password=sellsomeboxes;";
      //services.AddDbContext<SubscriptionContext>(options => options.UseSqlServer(subscriptionContext));
      var expenseContext = "Server=192.168.1.72;Database=ExpensesTest;Integrated Security=False;MultipleActiveResultSets=True;User ID=sa;Password=i1yuPtFOrIUjC7S4;";
      services.AddDbContext<ExpenseContext>(options => options.UseSqlServer(expenseContext));

      // Add framework services.
      services
        .AddMvcCore(options => {
          options.RequireHttpsPermanent = true; // does not affect api requests
          options.RespectBrowserAcceptHeader = true; // false by default
                                                     //options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

          //remove these two below, but added so you know where to place them...
          //options.OutputFormatters.Add(new YourCustomOutputFormatter());
          //options.InputFormatters.Add(new YourCustomInputFormatter());
        })
        //.AddApiExplorer()
        //.AddAuthorization()
        .AddFormatterMappings()
        //.AddCacheTagHelper()
        //.AddDataAnnotations()
        //.AddCors()
        .AddJsonFormatters()
        .AddJsonOptions(options => {
          options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }); // JSON, or you can build your own custom one (above)
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();
      
      app.UseMvc();
    }
  }
}
