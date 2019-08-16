using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyVideosWebApplication.CentralHub;
using MyVideosWebApplication.Model;
using Swashbuckle.AspNetCore.Swagger;

namespace MyVideosWebApplication
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddDbContext<scriberContext>();
      services.AddAutoMapper(typeof(Startup));


      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "TITLE", Version = "v1" });
      });

      services.AddCors(o => o.AddPolicy("MyPolicy", builder => {

        builder.AllowAnyOrigin()

               .AllowAnyMethod()

               .AllowAnyHeader();

      }));
      services.AddSignalR();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {

      app.UseCors(builder =>
      {
        builder.WithOrigins("https://jaemsa2019phase2test2.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
      });

      // SignalR
      app.UseFileServer();
      app.UseSignalR(routes =>
      {
        routes.MapHub<SignalrHub>("/hub");
      });
      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScribrAPI V1");
        c.RoutePrefix = string.Empty; // launch swagger from root
      });

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}
