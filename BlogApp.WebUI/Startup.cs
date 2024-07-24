using System.Threading.Tasks;
using BlogApp.DataAccess.Concrete.EntityFrameworkCore.Context;
using BlogApp.DataAccess.EntitFrameworkCore;
using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using BlogApp.DataAccess.EntitFrameworkCore.Concrete;
using BlogApp.Entity.Concrete;
using BlogApp.WebUI.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlogApp.WebUI {
    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddTransient<IArticleRepo, ArticleRepo>();
            services.AddTransient<ICategoryRepo, CategoryRepo>();
            services.AddTransient<IUserRolesRepo, UserClaimsRepo>();
            services.AddTransient<ILogRepo, LogRepo>();
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();

            services.AddDbContext<BlogDbContext>(_ => {
                _.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
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

            InitData.Init(userManager, roleManager);
        }
    }
}
