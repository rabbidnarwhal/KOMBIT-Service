using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KombitServer.Models;
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

    public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
      new LoggerFactory (new [] {
        new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider ()
      });
    public void ConfigureServices (IServiceCollection services) {
      string path = Path.Combine (Directory.GetCurrentDirectory (), "wwwroot");
      if (!Directory.Exists (path)) {
        Directory.CreateDirectory (path);
      }
      services.AddSingleton<IFileProvider> (
        new PhysicalFileProvider (path)
      );
      services.AddMvc ();
      services.AddSwaggerGen (s => {
        s.SwaggerDoc ("v1", new Info { Title = "KombitApi", Version = "v1" });
      });
      services.AddDbContext<KombitDBContext> (options =>
        options
        .UseLoggerFactory (_myLoggerFactory)
        .UseMySql (Configuration.GetConnectionString ("KombitDatabase")));
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
      app.UseStaticFiles ();
      app.UseMvc ();
      app.UseSwagger ();
      app.UseSwaggerUI (s => {
        s.SwaggerEndpoint ("/swagger/v1/swagger.json", "Kombit Api");
      });
    }
  }
}