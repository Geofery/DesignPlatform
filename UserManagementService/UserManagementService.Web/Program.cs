using UserManagementService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Management API",
        Version = "v1"
    });
});

// Configure NServiceBus using the configurator and pass the builder.Services
var endpointInstance = await NServiceBusConfigurator.ConfigureEndpoint("UserManagementService", builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapControllers();

// Ensure NServiceBus endpoint stops gracefully
app.Lifetime.ApplicationStopping.Register(async () => await endpointInstance.Stop().ConfigureAwait(false));

await app.RunAsync();
