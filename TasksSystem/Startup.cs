using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ClassLibrary;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Data;
using TasksSystem.Repos;
using TasksSystem.Services;
using AutoMapper;
using TasksSystem.Models;
using System.Xml;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Autofac;

namespace TasksSystem
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

            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config.xml"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            services.AddDbContext<ClassLibraryContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ClassLibraryContext")));
            services.AddControllersWithViews();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectRepository>();
            builder.RegisterType<ProjectService>();
            builder.RegisterType<UserRepository>();
            builder.RegisterType<UserService>();
            builder.RegisterType<ProjectUsersRepository>();
            builder.RegisterType<ProjectUsersService>();
            builder.RegisterType<TaskRepository>();
            builder.RegisterType<TaskService>();
            builder.RegisterType<CommentRepository>();
            builder.RegisterType<CommentService>();
            builder.RegisterType<RoleService>();
            builder.RegisterType<RoleRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
