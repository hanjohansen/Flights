using Flights.Client;
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
var app = builder.Build();

app.UseDatabaseMigrations();
app.UseStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapHub<RtcHub>(RtcHub.HubUrl);

app.Run();
