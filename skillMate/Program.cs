
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillMate.Data;
using SkillMate.Models;

namespace skillMate
{
    public class Program
    {
        public static void Main(string[] args)
        {


            

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // MVC
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed Roles and Admin Account
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Admin", "User" };

                foreach (var role in roles)
                {
                    bool roleExists = roleManager.RoleExistsAsync(role)
                        .GetAwaiter()
                        .GetResult();

                    if (!roleExists)
                    {
                        roleManager.CreateAsync(new IdentityRole(role))
                            .GetAwaiter()
                            .GetResult();
                    }
                }

                string adminEmail = "admin@skillmate.com";
                string adminPassword = "Admin123";

                var adminUser = userManager.FindByEmailAsync(adminEmail)
                    .GetAwaiter()
                    .GetResult();

                if (adminUser == null)
                {
                    var admin = new ApplicationUser
                    {
                        FullName = "System Admin",
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(admin, adminPassword)
                        .GetAwaiter()
                        .GetResult();

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "Admin")
                            .GetAwaiter()
                            .GetResult();
                    }
                }
            }

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";

                await next();
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Default Route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
