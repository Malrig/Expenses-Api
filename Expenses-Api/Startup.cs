using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using ExpensesApi.DAL;
using ExpensesApi.ViewModels;
using ExpensesApi.Services;
using ExpensesApi.Services.Expenses;
using ExpensesApi.Services.ExpenseLines;
using ExpensesApi.Validation;
using ExpensesApi.Validation.Expenses;
using ExpensesApi.Validation.ExpenseLines;

namespace ExpensesApi {
  /// <summary>
  /// Class which handles starting the app and configuring the services
  /// </summary>
  public class Startup {
    /// <summary>
    /// Configuration which is contained in the appsettings.json file
    /// </summary>
    public IConfigurationRoot configuration { get; }
    /// <summary>
    /// Configuration which is contained in the hosting.json file
    /// </summary>
    public IConfigurationRoot hostingConfiguration { get; }

    /// <summary>
    /// Constructor, gathers the configuration
    /// </summary>
    /// <param name="env"></param>
    public Startup(IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      configuration = builder.Build();

      var hostingBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json");
      hostingConfiguration = hostingBuilder.Build();
    }

    /// <summary>
    /// This method gets called by the runtime and is used to add services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services) {
      // Get the connection strings and whether to use an in memory database
      bool useInMemory = Convert.ToBoolean(configuration["UseInMemoryDatabase"]);
      string expenseConnectionString = configuration.GetConnectionString("ExpenseDatabase");

      //Either create the context in memory or using a connection string
      if ((!useInMemory) &&
          (expenseConnectionString != null) &&
          (expenseConnectionString != "")) {
        services.AddDbContext<ExpenseContext>(options => options.UseSqlServer(expenseConnectionString));
      }
      else {
        services.AddDbContext<ExpenseContext>(options => options.UseInMemoryDatabase());
      }

      // Configure JWT based authentication
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
          options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // Get the below information from configuration/secrets
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
          };
        });

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
        .AddFormatterMappings()
        //.AddCacheTagHelper()
        //.AddDataAnnotations()
        //.AddCors()
        .AddJsonFormatters()
        .AddJsonOptions(options => {
          options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }) // JSON, or you can build your own custom one (above)
        .AddApiExplorer() // Required for Swagger UI
        .AddAuthorization(); // Required for Identity to work

      // TODO - Add proper dependency injection for services.
      //services.AddScoped<IExpenseService, ExpenseService>();

      // Register the Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", 
                     new Info {
                       Title = "Expenses API",
                       Version = "v1"
                     });
        // Set the comments path for the Swagger JSON and UI.
        var basePath = AppContext.BaseDirectory;
        var xmlPath = Path.Combine(basePath, "Expenses-Api.xml");
        c.IncludeXmlComments(xmlPath);
      });

      ConfigureApplicationServices(services);
    }


    /// <summary>
    /// This method gets called by the runtime and is used to configure the HTTP request pipeline. 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="loggerFactory"></param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
      loggerFactory.AddConsole(configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      // Sets up the base path urls map correctly
      string basePath = Convert.ToString(hostingConfiguration["basePath"]);
      app.UsePathBase(basePath);
      
      // This needs to be added if the app is to be behind a Reverse-Proxy, it ensures that the 
      // app uses the forwarded headers. This is necessary to construct complete links etc.
      app.UseForwardedHeaders(new ForwardedHeadersOptions {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      // Enable authentication
      app.UseAuthentication();

      app.UseStaticFiles(); // TODO Check if this is required.
      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();
      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c => {
        c.SwaggerEndpoint(Path.Combine(basePath, "swagger/v1/swagger.json"), "My API V1");
      });
      app.UseMvc();
    }

    /// <summary>
    /// This function adds all the commands and queries used by the application
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureApplicationServices(IServiceCollection services) {
      // Add all queries as available services
      services
        .AddScoped<IQueryHandler<FindAllExpenses, ExpensesOverview>, GetExpensesOverview>()
        .AddScoped<IQueryHandler<FindExpenseById, ExpenseDetail>, GetExpenseDetail>();
        //.AddScoped<IQueryHandler<FindExpenseLinesByExpense, ExpenseLineList>, GetExpenseLinesForExpense>();

      // Add all commands which don't require validation
      services
        .AddScoped<ICommandHandler<DeleteExpenseInfo>, DeleteExpense>()
        .AddScoped<ICommandHandler<DeleteExpenseLineInfo>, DeleteExpenseLine>();

      // Add commands which have validators associated with them
      // Should be of the format Command, CommandInfo, Validator
      CommandHandlerRegistration
        .RegisterCommandHandler<AddExpense, AddExpenseInfo, AddExpenseValidator>(services);
      CommandHandlerRegistration
        .RegisterCommandHandler<UpdateExpense, UpdateExpenseInfo, UpdateExpenseValidator>(services);
      CommandHandlerRegistration
        .RegisterCommandHandler<AddUpdateExpense, AddUpdateExpenseInfo, AddUpdateExpenseValidator>(services);
      CommandHandlerRegistration
        .RegisterCommandHandler<AddUpdateExpenseLine, AddUpdateExpenseLineInfo, AddUpdateExpenseLineValidator>(services);
    }
  }
}
