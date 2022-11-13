using DisciplinarySystem.Infrastructure;
using DisciplinarySystem.Persistence;
using DisciplinarySystem.Persistence.Data.Initializer.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DisciplinarySystem.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration
        {
            get;
        }

        public Startup ( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public void ConfigureServices ( IServiceCollection services , IWebHostEnvironment env )
        {

            services.AddCors(options =>
            {
                options.AddPolicy("APICors" ,
                        builder => builder.WithOrigins("https://khedmat.razi.ac.ir")
                                          .AllowAnyMethod()
                                          .AllowAnyHeader());
            });


            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.Cookie.HttpOnly = false;
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                   options.LoginPath = "/Account/Login";
                   options.AccessDeniedPath = "/Home/AccessDenied";
                   options.SlidingExpiration = true;
               });

            services.AddHttpClient();
            services.AddHttpContextAccessor();


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services
                .AddPersistenceLayerDependencies(Configuration , env.IsDevelopment())
                .AddInfrastructureLayerDependencies()
                .AddApplicationLayerDependencies();

            services.AddSwaggerGen();
        }

        public void Configure ( WebApplication app , IWebHostEnvironment env , IDbInitializer dbInitializer )
        {
            if ( !app.Environment.IsDevelopment() )
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            if ( app.Environment.IsDevelopment() )
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseCors("APICors");

            app.UseAuthentication();
            app.UseAuthorization();

            dbInitializer.Execute().GetAwaiter().GetResult();

            app.UseSession();

            app.MapControllerRoute(
                name: "default" ,
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
