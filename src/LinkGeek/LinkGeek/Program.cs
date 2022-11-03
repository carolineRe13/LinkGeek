using LinkGeek.AppIdentity;
using LinkGeek.Data;
using LinkGeek.Hubs;
using LinkGeek.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

// Main program that is going to be executed at start

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
// adding swagger functionality to test api endpoints
builder.Services.AddSwaggerGen();

// Authentication from Azure
services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = configuration["ClientId"];
    microsoftOptions.ClientSecret = configuration["ClientSecret"];
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// sql server is used as a db of choice
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// adding all custom services
builder.Services
    .AddSingleton<IContextProvider, ContextProvider>()
    .AddSingleton<UserService>()
    .AddSingleton<DiscoverUserService>()
    .AddSingleton<GameDbService>()
    .AddSingleton<FriendService>()
    .AddSingleton<GameService>();

// snackbar functionality from MudBlazor
builder.Services.AddMudServices(c =>
{
    c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});
// Chat has to be a singleton to insure consistency
builder.Services.AddSingleton<ChatService>();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Run migrations on startup
using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;

    var context = servicesProvider.GetRequiredService<ApplicationDbContext>();    
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapHub<SignalRHub>("/signalRHub");
});

app.Run();