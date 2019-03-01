using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Hubs;
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
      this.serviceDBContext(services);
      this.serviceFileProvider(services);
      services.AddSignalR();

      this.serviceCors(services);
      
      this.serviceEmail(services);
      this.serviceScheduler(services);
      this.serviceSwagger(services);
    
      services.AddMvc ();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
      }


      this.configureFileProvider(app);
      this.configureSignalR(app);
      app.UseCors ("AllowAll");
      this.configurePathMiddleware(app);
      this.configureSwagger(app);
      
      app.UseMvc ();
      // app.UseMvcWithDefaultRoute();
    }

    private void serviceSwagger(IServiceCollection services) {
        services.AddSwaggerGen (c => {
        c.SwaggerDoc ("v1", new Info { Title = "KombitApi", Version = "v1" });
        var filePath = Path.Combine (System.AppContext.BaseDirectory, "KombitServer.xml");
        c.IncludeXmlComments (filePath);
      });
    }

    private void serviceFileProvider(IServiceCollection services) {
      string path = Path.Combine (Directory.GetCurrentDirectory (), "assets");
      if (!Directory.Exists (path)) {
        Directory.CreateDirectory (path);
      }
      services.AddSingleton<IFileProvider> (
        new PhysicalFileProvider (path)
      );
    }

    private void serviceDBContext(IServiceCollection services) {
      services.AddDbContext<KombitDBContext> (options =>
        options.UseMySql (Configuration.GetConnectionString ("KombitDatabase"))
      );
    }

    private void serviceScheduler(IServiceCollection services) {
      services.AddSingleton<IScheduledTask, CheckProductUpdate>();
      services.AddScheduler((sender, args) =>
      {
          Console.Write(args.Exception.Message);
          args.SetObserved();
      });
    }

    private void serviceEmail(IServiceCollection services) {
      services.AddSingleton<IEmailService, EmailService>();
      services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
    }

    private void serviceCors(IServiceCollection services) {
      services.AddCors(o => {
        o.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
      });
    }

    private void configureSwagger(IApplicationBuilder app) {
      app.UseSwagger (c => {
        c.RouteTemplate = "docs/{documentName}/swagger.json";
      });
      app.UseSwaggerUI (c => {
        c.SwaggerEndpoint ("/docs/v1/swagger.json", "Kombit Api");
      });
    }

    private void configureFileProvider(IApplicationBuilder app) {
      app.UseDefaultFiles();
      app.UseStaticFiles ();
      app.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "assets")),
        RequestPath = ""
      });
    }

    private void configurePathMiddleware(IApplicationBuilder app) {
      app.Use(async (context, next) => {
        await next();
        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.Value.StartsWith("/api") && !context.Request.Path.Value.StartsWith("/hub")) {
          context.Request.Path = "/index.html";
          context.Response.StatusCode = 200;
          await next();
        }
      });
    }

    private void configureSignalR(IApplicationBuilder app) {
      app.UseSignalR(route => {
        route.MapHub<ChatHub> ("/hub/chat");
      });
    }
  }
}