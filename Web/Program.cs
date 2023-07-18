using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using System.Runtime.Loader;
using Poplike.Application.Account.BackgroundServices;
using Poplike.Application.Emails.BackgroundServices;
using Poplike.Application.Sessions.BackgroundServices;
using Poplike.Application.Sessions.Queues;
using Poplike.Common.Settings;
using Poplike.Persistence;

namespace Poplike.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuration
        builder.Configuration
            .AddJsonFile("Config/Database.json", optional: false, reloadOnChange: true)
            .AddJsonFile("Config/Email.json", optional: false, reloadOnChange: true);

        var connectionStringFactory = new ConnectionStringFactory(
            builder.Environment,
            builder.Configuration);

        var connectionString = connectionStringFactory.GetConnectionString();

        builder.Services.Configure<EmailAccountConfiguration>(
            builder.Configuration.GetSection("Features:Email:Accounts:Mailout"));

        builder.Services.Configure<UserAccountConfiguration>(
            builder.Configuration.GetSection("User:Account"));

        // Dynamic dependency injection
        var files = Directory.GetFiles(
            AppDomain.CurrentDomain.BaseDirectory,
            "Poplike*.dll");

        var assemblies = files
            .Select(p => AssemblyLoadContext.Default.LoadFromAssemblyPath(p));

        builder.Services
            .Scan(p => p.FromAssemblies(assemblies)
            .AddClasses()
            .AsMatchingInterface());

        // Services
        var contentRootPath = $"{builder.Environment.ContentRootPath}\\SessionKeys";

        builder.Services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(contentRootPath));

        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/help/notpermitted";
                options.LoginPath = "/Session/SignIn";
                options.LogoutPath = "/Session/SignOut";
                options.EventsType = typeof(CookieValidator);
            });

        builder.Services.AddSingleton<ISessionActivityList, SessionActivityList>();

        builder.Services.AddScoped<CookieValidator>();
        builder.Services.AddScoped<IUserToken, UserToken>();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = options.DefaultPolicy;
        });

        builder.Services.AddDbContext<DatabaseService>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        if (builder.Environment.IsProduction())
        {
            builder.Services.AddHostedService<EmailSender>();
            builder.Services.AddHostedService<PasswordResetReaper>();
            builder.Services.AddHostedService<SessionActivityLogger>();
            builder.Services.AddHostedService<SessionReaper>();
        }

        // Localization
        builder.Services.AddLocalization();
            // options => options.ResourcesPath = "Resources/");
            // Note:
            // Commented out, since we're not using any .resx resource files.

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("sv-SE"),
            };

            options.DefaultRequestCulture = new RequestCulture("sv-SE");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        // Razor
        builder.Services.AddRazorPages();

        // App
        var app = builder.Build();

        // Middleware
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");

            // The default HSTS value is 30 days.
            // You may want to change this for production scenarios.
            // https://aka.ms/aspnetcore-hsts.

            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseCookiePolicy(
            new CookiePolicyOptions
            {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict,
                Secure = CookieSecurePolicy.Always,
            });

        app.UseRouting();

        var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value;
        app.UseRequestLocalization(localizationOptions);
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        // Run
        app.Run();
    }
}
