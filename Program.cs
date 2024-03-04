using prof_tester_api;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5050");

var startUp = new Startup(builder.Configuration);
startUp.ConfigureServices(builder.Services);

var app = builder.Build();
startUp.Configure(app);
