using DisciplinarySystem.Persistence.Data.Initializer.Interfaces;
using DisciplinarySystem.Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSerilog(( builder , logger ) =>
{
    if ( builder.HostingEnvironment.IsDevelopment() )
    {
        logger.WriteTo.Console().MinimumLevel.Information();
    }
});


builder.Host.ConfigureAppConfiguration(( hostingContext , config ) =>
{

    var env = hostingContext.HostingEnvironment;

    config.AddJsonFile("appsettings.json" , optional: true , reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json" , optional: true , reloadOnChange: true);

});

var startUp = new Startup(builder.Configuration);
startUp.ConfigureServices(builder.Services , builder.Environment);

var app = builder.Build();

var scoped = app.Services.CreateScope();
var dbInitializer = scoped.ServiceProvider.GetService<IDbInitializer>();

startUp.Configure(app , builder.Environment , dbInitializer);
app.Run();
