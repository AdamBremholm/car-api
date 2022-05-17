using CarConsole.Extensions;
using CarConsole.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

//ConfigureServices
builder.Services.AddEndpointDefinitions(typeof(Car));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Car>());
// Adds Authorize button in Swagger

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

// Adds Authentication for all endpoints
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});
var app = builder.Build();

//Configure
app.UseEndpointDefinitions();
app.UseAuthentication();
app.UseAuthorization();


app.Run();
