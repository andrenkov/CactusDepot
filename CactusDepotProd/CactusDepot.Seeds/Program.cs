using CactusDepot.Shared;
using CactusDepot.Seeds.Controllers;
using CactusDepot.Shared.DataContext;
using CactusDepot.Shared.Models.Administration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CactusDepot.Common.Models.Models.Healthcheck;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

#region WebApplicationBuilder builder 
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Read Config settings
IConfiguration configuration = builder.Configuration;//  provider.GetService<IConfiguration>();
#region MySql Conn
//see https://www.connectionstrings.com/mysql/
//use host,port when connected to a nother container

#region admin conn
//The configuration environment variables for our MySql Server will be supplied in our docker-compose file when it is initialized,
string hostAddress = configuration["DBHOSTADMIN"] ?? "localhost";
string port = configuration["DBPORTADMIN"] ?? "3306";

string password = configuration["MYSQLADMIN_PASSWORD"] ?? configuration.GetConnectionString("MYSQLADMIN_PASSWORD");
string userid = configuration["MYSQLADMIN_USER"] ?? configuration.GetConnectionString("MYSQLADMIN_USER");

string usersDataBase = configuration["MYSQLADMIN_DATABASE"] ?? configuration.GetConnectionString("AdminConnection");

//string connUsers = $"server={hostAddress};port={port};protocol=tcp;userid={userid};pwd={password};database={usersDataBase};";
string connUsers = $"server={hostAddress},{port};protocol=tcp;userid={userid};pwd={password};database={usersDataBase};ConnectionIdleTimeout=1800;Pooling=true;ConnectionLifeTime=1800;MaximumPoolsize=100;ConnectionTimeout=5;";
//"ConnectionIdleTimeout=1800;Pooling=true;ConnectionLifeTime=1800;MaximumPoolsize=100;ConnectionTimeout=30;";

SharedUtil.WriteLogToConsole("Config", "Config loaded");
#endregion

#region seeds conn
//The configuration environment variables for our MySql Server will be supplied in our docker-compose file when it is initialized,
hostAddress = configuration["DBHOSTSEEDS"] ?? "localhost";
port = configuration["DBPORTSEEDS"] ?? "3306";

password = configuration["MYSQLSEEDS_PASSWORD"] ?? configuration.GetConnectionString("MYSQLSEEDS_PASSWORD");
userid = configuration["MYSQLSEEDS_USER"] ?? configuration.GetConnectionString("MYSQLSEEDS_USER");

string seedsDataBase = configuration["MYSQLSEEDS_DATABASE"] ?? configuration.GetConnectionString("SeedsConnection");

//string connSeeds = $"server={hostAddress};port={port};protocol=tcp;userid={userid};pwd={password};database={seedsDataBase};";
string connSeeds = $"server={hostAddress},{port};protocol=tcp;userid={userid};pwd={password};database={seedsDataBase};ConnectionIdleTimeout=1800;Pooling=true;ConnectionLifeTime=1800;MaximumPoolsize=100;ConnectionTimeout=5;";
//"ConnectionIdleTimeout=1800;Pooling=true;ConnectionLifeTime=1800;MaximumPoolsize=100;ConnectionTimeout=30;";
//Uid=;Pwd=
#endregion

//string CorsOrigins = configuration["CORSORIGINS"];
//string UseUrls = configuration["USEURLS"];
#endregion

#endregion

#region DI for DbContext
//builder.Services.AddDbContext<SeedsDbContext>(options =>
//                                                 options.UseMySql(connSeeds, new MySqlServerVersion(new Version(8, 0, 29)))
//                                                 .EnableDetailedErrors()
//                                                 .EnableServiceProviderCaching()
//                                                 );//ServerVersion.AutoDetect(connSeeds)));--if use a latest mysql
SharedUtil.WriteLogToConsole("DbContext", "AddDbContextPool<SeedsDbContext> start");
builder.Services.AddDbContextPool<SeedsDbContext>(options =>
{
    options.UseMySql(connSeeds,
        ServerVersion.AutoDetect(connSeeds),
        mySqlOptions =>
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,                      //The maximum number of retry attempts (5)
                maxRetryDelay: TimeSpan.FromSeconds(5),//The maximum delay between retries. was 30
                errorNumbersToAdd: null)               //Additional error codes that should be considered transient.
            )
     .EnableDetailedErrors()
     .EnableServiceProviderCaching();
});

SharedUtil.WriteLogToConsole("DbContext", "AddDbContextPool<SeedsDbContext> complete");


//builder.Services.AddDbContext<UserDbContext>(options =>
//                                                options.UseMySql(connUsers, new MySqlServerVersion(new Version(8, 0, 29)))
//                                                 .EnableDetailedErrors()
//                                                 .EnableServiceProviderCaching()
//
//  );//ServerVersion.AutoDetect(connUsers)));--if use a latest mysql
SharedUtil.WriteLogToConsole("DbContext", "AddDbContextPool<UserDbContext> start");
builder.Services.AddDbContextPool<UserDbContext>(options =>
{
    options.UseMySql(connUsers,
        ServerVersion.AutoDetect(connUsers),
        mySqlOptions =>
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,                     //The maximum number of retry attempts (5)
                maxRetryDelay: TimeSpan.FromSeconds(5),//The maximum delay between retries. was 30
                errorNumbersToAdd: null)               //Additional error codes that should be considered transient.  
            )
     .EnableDetailedErrors()
     .EnableServiceProviderCaching();
});
SharedUtil.WriteLogToConsole("DbContext", "AddDbContextPool<UserDbContext> complete");
#endregion

# region Healthcheck
//
//Add AspNetCore.HealthChecks.MySql
//
builder.Services.AddHealthChecks()
    .AddMySql(connSeeds, 
     name: "seedDb",
     failureStatus: HealthStatus.Unhealthy,
     tags: new[] { "db", "mysql", "mysqlserver" })
    .AddMySql(connUsers,
     name: "userDb",
     failureStatus: HealthStatus.Unhealthy,
     tags: new[] { "db", "mysql", "mysqlserver" })
    .AddCheck<HealthcheckSrv>(
                                "HealthcheckSrv",
                                 failureStatus: HealthStatus.Degraded,
                                 tags: new[] { "basicSRVcheck" }
                             );

//builder.Services.AddHealthChecks()
//    .AddDbContextCheck<UserDbContext>();

builder.Services.AddHealthChecksUI()
    //( setupSettings =>
    //setupSettings.AddHealthCheckEndpoint("https", new UriBuilder("http", "avlad.no-ip.info", 9091, "/health.json").ToString()))
    .AddInMemoryStorage();

#endregion

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#region Identity
SharedUtil.WriteLogToConsole("DbContext", "AddDefaultIdentity<IdentityUser> start");
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    //sign in options
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>();
SharedUtil.WriteLogToConsole("DbContext", "AddDefaultIdentity<IdentityUser> complete");
SharedUtil.WriteLogToConsole("ApplicationCookie", "ConfigureApplicationCookie() start");
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
SharedUtil.WriteLogToConsole("ApplicationCookie", "ConfigureApplicationCookie() complete");
builder.Services.AddAuthorization(options => { options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator")); });
SharedUtil.WriteLogToConsole("Identity", "AddAuthorization complete");
#endregion

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
#endregion

#region Add Seeds Sevice
//Just AddScoped() will work. TryAdd protects from duplicate services being registered.
//For multiple interfaces of the same service, use TryAddEnumerable()
SharedUtil.WriteLogToConsole("DbContext", "TryAddScoped<ISeedsRepository start");
builder.Services.TryAddScoped<ISeedsRepository, SQLSeedsRepository>();
SharedUtil.WriteLogToConsole("DbContext", "TryAddScoped<ISeedsRepository complete");
//added this for View with injection (testing only).
//Need this in addition to above (services.TryAddScoped<ISeedsRepository, SQLSeedsRepository>();)
//to call endpoint SeedsCount() which is not in the ISeedsRepository

//Transient objects are always different; a new instance is provided to every controller and every service.
//Scoped objects are the same within a request, but different across different requests.
//Singleton objects are the same for every object and every request.
SharedUtil.WriteLogToConsole("DbContext", "TryAddTransient<SQLSeedsRepository> start");
builder.Services.TryAddTransient<SQLSeedsRepository>();
SharedUtil.WriteLogToConsole("DbContext", "TryAddTransient<SQLSeedsRepository> complete");
#endregion

#region Listen Port (works for API)
//UseUrls stops website from responding.But can be used for API endpoints OK along with CORSORIGINS
//builder.WebHost.UseUrls(UseUrls);

//for logs etc. Might want to test SeriLog from NuGet
//builder.WebHost.UseKestrel();

//ConfigureKestrel is important for hosting on a remote server like production
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    //can be  fixed it is by specifying the cert and password liek:
//    //-e ASPNETCORE_Kestrel__Certificates__Default__Path="name_of_file.pfx" -e ASPNETCORE_Kestrel__Certificates__Default__Password="password_here"
//    serverOptions.ListenAnyIP(443, listenOptions =>
//    {
//        listenOptions.UseHttps("9090");
//    });
//    serverOptions.ListenAnyIP(9091);
//    serverOptions.ListenAnyIP(80); // NOTE: optionally listen on port 80, too
//});

//NOTE: optionally, use HTTPS redirection
//builder.Services.AddHttpsRedirection(options =>
//{
//    options.RedirectStatusCode = (int)System.Net.HttpStatusCode.PermanentRedirect; // CAN ALSO USE System.Net.HttpStatusCode.TemporaryRedirect
//options.HttpsPort = 443;
//});
#endregion

#region Logs
builder.Services.AddLogging(builder =>
                builder
                    .AddDebug()
                    .AddConsole()
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .SetMinimumLevel(LogLevel.Information)
                    //below doesn't affect logging???
                    //.AddJsonConsole(options =>
                    //{
                    //    options.IncludeScopes = true;
                    //    //options.SingleLine = true;
                    //    options.TimestampFormat = "hh:mm:ss ";
                    //    options.JsonWriterOptions = new JsonWriterOptions
                    //    {
                    //        Indented = true
                    //    };
                    //}
                    //)
                    );
#endregion

#region WebApplication app
WebApplication app = builder.Build();

#region Seeding Data (Users, Rolses)
//below is the code to seed the Db with default Roles and Users
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    ILoggerFactory loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        UserManager<IdentityUser> userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await InitAccountData.SeedRolesAsync(userManager, roleManager);
        await InitAccountData.SeedSuperAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
SharedUtil.WriteLogToConsole("DbContext", "Seeding Data complete");
#endregion

#region HTTP routing
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseDatabaseErrorPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");//("/Home/Error");
                                      //app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//not working for publishing to a Host without an SSL Cert
//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#region Healthcheck
// default check
//app.MapHealthChecks("/healthz"); 
//custom check
app.MapHealthChecks("/healthz", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
)
//.RequireHost("www.avlad.no-ip.info:9091")
//.RequireAuthorization();
;
app.MapHealthChecksUI()
    //.RequireHost("http://avlad.no-ip.info:9091")
    ;
#endregion

#endregion

app.MapRazorPages();

SharedUtil.WriteLogToConsole("app", "app.Run()");
app.Run();
#endregion



