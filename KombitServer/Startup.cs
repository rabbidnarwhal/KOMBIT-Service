using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
using KombitServer.Models.Email;
using KombitServer.ScheduleTask;
using KombitServer.ScheduleTask.Scheduling;
using KombitServer.ScheduleTask.Tasks;
using KombitServer.Services.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace KombitServer {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.

    public void ConfigureServices (IServiceCollection services) {
      string path = Path.Combine (Directory.GetCurrentDirectory (), "assets");
      if (!Directory.Exists (path)) {
        Directory.CreateDirectory (path);
      }
      services.AddSingleton<IFileProvider> (
        new PhysicalFileProvider (path)
      );
      services.AddMvc ();
      services.AddSwaggerGen (c => {
        c.SwaggerDoc ("v1", new Info { Title = "KombitApi", Version = "v1" });
        var filePath = Path.Combine (System.AppContext.BaseDirectory, "KombitServer.xml");
        c.IncludeXmlComments (filePath);
      });
      services.AddDbContext<KombitDBContext> (options =>
        options
        .UseMySql (Configuration.GetConnectionString ("KombitDatabase")));
      // services.AddSpaStaticFiles(configuration => {
      //   configuration.RootPath = "ClientApp/dist";
      // })
      services.AddSingleton<IScheduledTask, CheckProductUpdate>();
      services.AddScheduler((sender, args) =>
      {
          Console.Write(args.Exception.Message);
          args.SetObserved();
      });
      services.AddSingleton<IEmailService, EmailService>();
      services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      }
      app.UseCors (option => {
        option.AllowAnyOrigin ();
        option.AllowAnyMethod ();
        option.AllowAnyHeader ();
      });
      app.Use(async (context, next) => {
        await next();
        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.Value.StartsWith("/api")) {
          context.Request.Path = "/index.html";
          context.Response.StatusCode = 200;
          await next();
        }
      });
      app.UseMvcWithDefaultRoute();
      app.UseDefaultFiles();
      app.UseStaticFiles ();
      app.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "assets")),
        RequestPath = ""
      });
      app.UseMvc ();
      app.UseSwagger (c => {
        c.RouteTemplate = "docs/{documentName}/swagger.json";
      });
      app.UseSwaggerUI (c => {
        c.SwaggerEndpoint ("/docs/v1/swagger.json", "Kombit Api");
      });
    }
  }
}