using Mfm.Bc.CurrencyExchanger.WebApi.App_Code.Middlewares;
using Mfm.Bc.CurrencyExchanger.WebApi.App_Code.ServiceExtensions;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the order of configuration sources
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);


builder.Services.AddFrankfurterServices(frankfurterAppConfig =>
{
    builder.Configuration.GetSection("FrankfurterAppConfig").Bind(frankfurterAppConfig);
});
builder.Services.RegisterDomainServices();
builder.Services.RegisterConfigs(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(builder.Configuration);

// Adding Rate Limiting
builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter("Default", opt => {
        opt.Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:Default:WindowInSeconds"));
        opt.PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:Default:PermitLimit"); 
    });
    //Too many requests
    options.RejectionStatusCode = 429;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
        options.EnableDeepLinking();
    }
    );
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.Run();
