using CarConsole.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//ConfigureServices
builder.Services.AddSingleton<IRepository, DictionaryRepository>(opts => new DictionaryRepository(new Dictionary<Guid, Car>()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Car>());
// Adds Authorize button in Swagger
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
         Id = "Bearer",
         Type = ReferenceType.SecurityScheme
        }
      }, new List<string>()
    }

    });

});
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
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/cars", (IRepository repo) =>
    {
        var cars = repo.GetAll();
        return Results.Ok(cars);
    })
  // .RequireAuthorization() If not enabled by default
  .AllowAnonymous() // Removes authentication for endpoint
  .Produces<List<Car>>();

app.MapGet("/cars/{id}", (IRepository repo, Guid id) =>
    {
        var car = repo.GetById(id);
        return car is not null ? Results.Ok(car) : Results.NotFound();
    }).Produces<Car>();

app.MapPost("/cars", (Car car, IRepository repo, IValidator<Car> validator) =>
    {
        var validationResult = validator.Validate(car);
        if (!validationResult.IsValid)
        {
          var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
          return Results.BadRequest(errors);
        }
        repo.Create(car);
        return Results.Created($"/cars/{car.Id}", car);
    })
.AllowAnonymous()
.Produces<Car>();

app.MapPut("/cars/{id}", (Car updatedCar, IRepository repo, Guid id) =>
    {
        var car = repo.GetById(id);
        if (car is null)
        {
            return Results.NotFound();
        }

        repo.Update(updatedCar);
        return Results.Ok(updatedCar);

    }).Produces<Car>();

app.MapDelete("/cars/{id}", (IRepository repo, Guid id) =>
    {
        repo.Delete(id);
        return Results.NoContent();
    });

app.Run();
