Install MySql with users and network conf changes toi allow remote connections (see Mysql notes).
Create Models and DbContext classes.
Add refewrences to:
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="6.0.4" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.4">
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="6.0.1" />

!!! CORE 6.0 and Startup.cs
For advanced Services, Db seeding, User and Role management (options etc.), it's better to use the Startup.cs move the host configuration from Program.cs 
to it with ConfigureWebHostDefaults() etc.


Use of SeedsDbContextExtensions() seems has problem with EF so keep it simple for Migrations and change back later on.
    builder.Services.AddDbContext<SeedsDbContext>(options =>
                                                options.UseMySql(connSeeds, ServerVersion.AutoDetect(connSeeds)));
Run PM>Install-Package Microsoft.EntityFrameworkCore (if not installed)

!!!
Set the Web App as the solution's startup project. But in Package Mng Console select the DataContext project.

1. EntityFrameworkCore\Add-Migration InitialMigrationSeeds -Context SeedsDbContext
2. EntityFrameworkCore\Add-Migration InitialMigrationUsers -Context UserDbContext
3. EntityFrameworkCore\Update-Database -Context SeedsDbContext
4. EntityFrameworkCore\Update-Database -Context UserDbContext

logins

andrenkov@gmail.com/C@tal0g2022
HASH AQAAAAEAACcQAAAAEF4WetK6Rb0Es4rl5x0dROSeLH85RzttMek49IzIQOEjNj55BpaM/U+REoeQ0OwWgw==
andrenkova@gmail.com/C@tal0g2022
HASH AQAAAAEAACcQAAAAEAFDGWEh2vz7JXcj0q6iMCdHioFrz0ML5hd5LUbGU1j3QegQUhMQ9eyXYmemQ2Pcdw==

To read:

https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-6.0
https://entityframeworkcore.com/knowledge-base/60282522/cannot-login-to-seeded-custom-identityuser-from-passwordhasher

Seeding User, use UserName = "andrenkov@gmail.com"//email, not the Name !!!!!!!!!!!!!!!

ApplicationUser
IdentityUser

Using the Name instead of the email address:
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-6.0&tabs=visual-studio&viewFallbackFrom=aspnetcore-2.2

How do you scaffold identity pages?
    From Solution Explorer, right-click on the project > Add > New Scaffolded Item. 
    From the left pane of the Add Scaffold dialog, select Identity > Add. 
    In the Add Identity dialog, select the options you want. 
    Select your existing layout page so your layout file isn't overwritten with incorrect markup.
    Packaghe required is Microsoft.VisualStudio.Web.CodeGeneration.Design. Make sure that it's the same version as other Core packages

#####################
Docker
#####################

For example of the docker support, see solution DockerComposeTemplates

1. Add Microsoft.VisualStudio.Shell.Interop.DesignTime package 