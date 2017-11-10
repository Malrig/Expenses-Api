using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;

using ExpensesApi.DAL;
using ExpensesApi.Identity.DAL;
using ExpensesApi.Identity.Models;
using ExpensesApi.Identity.Utilities;
using ExpensesApi.Services;

namespace ExpensesApi {
  public class Startup {
    const string TokenAudience = "ExampleAudience";
    const string TokenIssuer = "ExampleIssuer";
    private RsaSecurityKey key;
    private TokenAuthOptions tokenOptions;

    public IConfigurationRoot Configuration { get; }

    public Startup(IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();

      Configuration = builder.Build();
    }


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
        services.AddDbContext<IdentityContext>(options => options.UseSqlServer(identityConnectionString));
      }
      else {
        services.AddDbContext<ExpenseContext>(options => options.UseInMemoryDatabase());
        services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase());
      }

      // Set up the JWT Bearer Tokens
      // This part of the code heavily taken from 
      // https://github.com/mrsheepuk/ASPNETSelfCreatedTokenAuthExample/tree/master/src/TokenAuthExampleWebApplication
      // TODO look into changing this so that it doesn't regenerate everytime the app is restarted.
      RSAParameters keyParams = RSAKeyUtils.GetRandomKey();

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
        .AddApiExplorer() // Required for Swagger UI
        .AddAuthorization(); // Required for Identity to work
        
      // TODO - Add proper dependency injection for services.
      //services.AddScoped<IExpenseService, ExpenseService>();

      // Register the Identity service
      services
        .AddIdentity<ApplicationUser, IdentityRole<int>>(options => {
        // Set the paths for the AccessDenied and Login pages 
        // (will be actions not pages as this is an API)
        options.Cookies.ApplicationCookie.AccessDeniedPath = "/accessdenied";
        options.Cookies.ApplicationCookie.LoginPath = "/login";
        options.Cookies.ApplicationCookie.LoginPath = "/logout";

        // TODO Work out exactly what these event handlers are doing.
        options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents() {
          OnRedirectToAccessDenied = context => {
            if (context.Request.Path.StartsWithSegments("/api")) {
              context.Response.StatusCode = 403;
              return Task.FromResult(0);
            }

            context.Response.Redirect(context.RedirectUri);
            return Task.FromResult(0);

          },
          OnRedirectToLogin = context =>
          {
            if (context.Request.Path.StartsWithSegments("/api")) {
              context.Response.StatusCode = 401;
              return Task.FromResult(0);
            }

            context.Response.Redirect(context.RedirectUri);
            return Task.FromResult(0);
          }
        };
      })
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

      app.UseStaticFiles(); // TODO Check if this is required.
      // Enable use of JSON Web Tokens
      app.UseJwtBearerAuthentication(new JwtBearerOptions {
        AutomaticAuthenticate = true,
        AutomaticChallenge = true,
        TokenValidationParameters = new TokenValidationParameters {
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppConfiguration:Key").Value)),
          ValidAudience = Configuration.GetSection("AppConfiguration:SiteUrl").Value,
          ValidateIssuerSigningKey = true,
          ValidateLifetime = true,
          ValidIssuer = Configuration.GetSection("AppConfiguration:SiteUrl").Value
        }
      });
      // Enable middleware for identity
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
