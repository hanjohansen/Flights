using Flights.Client;
using Flights.Client.Rtc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddGamesDatabase();
builder.AddFileStorage();
builder.AddSignalRServices();
builder.AddUiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseDatabaseMigrations();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapHub<RtcHub>(RtcHub.HubUrl);

app.Run();
