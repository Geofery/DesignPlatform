using UserManagementService.Domain;
using UserManagementService.Infrastructure;
using UserManagementService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Logging.AddSimpleConsole(console => {
    console.IncludeScopes = true;
    console.TimestampFormat = "HH:mm:ss.ffff ";
    //console.SingleLine = true;
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "User Management API",
        Version = "v1"
    });
});

var services = builder.Services;
var configuration = builder.Configuration;

// --------------------------------------------------------------------------------------
// Configure Services (DI setup)
// --------------------------------------------------------------------------------------

// Add the UserRepository registration
services.AddTransient<IUserRepository, UserRepository>();


// Configure NServiceBus using the configurator and pass the builder.Services
//var endpointInstance = await NServiceBusConfigurator.ConfigureEndpoint("UserManagementService", builder.Services);
//var endpointInstance =  NServiceBusConfigurator.ConfigureAndStartEndpoint("UserManagementService", builder.Services);
var endpointInstance =  NServiceBusConfigurator.ConfigureAndStartEndpoint("UserManagementService", services);



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
