using BlogApp.DataAccess.Abstract;
using BlogApp.DataAccess.Concrete;
using BlogApp.Entity.Concrete;
using BlogApp.DataAccess.Concrete.EntityFramework;
using BlogApp.WebUI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace BlogApp.WebUI {
    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddTransient<IBlogDal, EfBlogDal>();
            services.AddTransient<ICategoryDal, EfCategoryDal>();
            services.AddTransient<IUserRolesDal, EfUserClaimsDal>();
            services.AddTransient<ILogDal, EfLogDal>();
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();

            services.AddDbContext<BlogDbContext>(_ => {
                _.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //_.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddIdentity<AppUser, IdentityRole>(_ => {
                _.Password.RequiredLength = 3;
                _.Password.RequireLowercase = false;
                _.Password.RequireUppercase = false;
                _.Password.RequireNonAlphanumeric = false;
                _.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<BlogDbContext>()
              .AddDefaultTokenProviders();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {

            DeveloperExceptionPageOptions pageOptions = new DeveloperExceptionPageOptions { SourceCodeLineCount = 10 };

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage(pageOptions);
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStatusCodePages(context => {

                var isAjax = context.HttpContext.Request.Headers.Keys.Contains("x-requested-with");
                if (isAjax) {
                    return Task.CompletedTask;
                }

                var response = context.HttpContext.Response;
                response.Redirect($"/Error/{response.StatusCode}");

                return Task.CompletedTask;
            });

            app.UseMvcWithDefaultRoute();

            SeedData.Seed(userManager, roleManager);
        }
    }
}
