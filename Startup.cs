using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PomodoroApp.Extensions;
using PomodoroApp.Models;
using PomodoroApp.Settings;
using Newtonsoft.Json;
using Microsoft.AspNetCore.JsonPatch;
using Pomodoroapp.Models;
using pomodoroapp.Models;
using pomodoroapp.Modules;
using Microsoft.AspNetCore.Rewrite;

namespace PomodoroApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {
          

            services.AddDbContext<EFDataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

          



            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;


            }).AddEntityFrameworkStores<EFDataContext>()
            .AddDefaultTokenProviders();

            services.AddCors();
            services.AddTransient<IDataRepository, EFDataRepository>();
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddTransient<JwtGenerator>();
            services.AddAuth(Configuration.GetSection("Jwt").Get<JwtSettings>());







            services.AddHostedService<TimerModule>();


     

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
       

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           

            app.UseRouting();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseCors(builder => builder.WithOrigins( "http://pomodorotracker.ru", "https://pomodorotracker.ru","http://localhost:3000")
            .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            endpoints.MapControllers()
            );
        }
    }
}
