using System;
using System.IO;
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
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using ExpensesApi.DAL;
using ExpensesApi.Identity;
using ExpensesApi.Identity.DAL;
using ExpensesApi.Services;

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
      // Get the connection strings and whether to use an in memory database
      bool useInMemory = Convert.ToBoolean(Configuration["UseInMemoryDatabase"]);
      string expenseConnectionString = Configuration.GetConnectionString("expense");
      string identityConnectionString = Configuration.GetConnectionString("identity");

      //Either create the context in memory or using a connection string
      if ((!useInMemory) &&
          (expenseConnectionString != null) &&
          (expenseConnectionString != "") &&
          (identityConnectionString != null) &&
          (identityConnectionString != "")) {
        services.AddDbContext<ExpenseContext>(options => options.UseSqlServer(expenseConnectionString));
        services.AddDbContext<ExpenseContext>(options => options.UseSqlServer(identityConnectionString));
      }
      else {
        services.AddDbContext<ExpenseContext>(options => options.UseInMemoryDatabase());
        services.AddDbContext<ExpenseContext>(options => options.UseInMemoryDatabase());
      }

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
        }) // JSON, or you can build your own custom one (above)
        .AddApiExplorer(); // Required for Swagger UI

      // TODO - Add proper dependency injection for services.
      //services.AddScoped<IExpenseService, ExpenseService>();

      // Register the Identity service
      services.AddIdentity<ApplicationUser, IdentityRole<int>>()
              .AddEntityFrameworkStores<IdentityContext, int>()
              .AddDefaultTokenProviders();

      // Register the Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
        // Set the comments path for the Swagger JSON and UI.
        var basePath = PlatformServices.Default.Application.ApplicationBasePath;
        var xmlPath = Path.Combine(basePath, "Expenses-Api.xml");
        c.IncludeXmlComments(xmlPath);
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      // Enable middleware for identity
      app.UseStaticFiles(); // TODO Check if this is required.
      app.UseIdentity();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();
      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });

      app.UseMvc();
    }
  }
}
