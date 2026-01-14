using Flights.Client;
using Flights.Client.Configuration.Logging;
using Flights.Client.Rtc;

var builder = WebApplication.CreateBuilder(args);

// setup
builder.AddAuth();
builder.AddDataProtectionApi();
builder.AddGamesDatabase();
builder.AddFileStorage();
builder.AddSignalRServices();
builder.AddUiServices();

// run
builder.AddLogging();
builder.LogConfiguration();

var app = builder.Build();

app.UseDatabaseMigrations();

app.UseMiddleware<RequestExceptionMiddleware>();
app.UseStaticAssets();
app.UseAntiforgery();
app.UseRequestLogging();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapHub<RtcHub>(RtcHub.HubUrl);

app.Run();
