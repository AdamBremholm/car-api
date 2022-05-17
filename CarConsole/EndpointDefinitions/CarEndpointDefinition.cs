using CarConsole.Models;
using FluentValidation;

namespace CarConsole.EndpointDefinitions;

public class CarEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/cars", (IRepository repo) => GetAll(repo))
          // .RequireAuthorization() If not enabled by default
          .AllowAnonymous() // Removes authentication for endpoint
          .Produces<List<Car>>();

        app.MapGet("/cars/{id}", (IRepository repo, Guid id) => GetById(repo, id))
        .Produces<Car>();

        app.MapPost("/cars", (Car car, IRepository repo, IValidator<Car> validator) => Create(repo, car, validator))
        .AllowAnonymous()
        .Produces<Car>();

        app.MapPut("/cars/{id}", (Car updatedCar, IRepository repo, Guid id) => Update(repo, updatedCar, id))
          .Produces<Car>();

        app.MapDelete("/cars/{id}", (IRepository repo, Guid id) => Delete(repo, id));
    }

    internal IResult GetById(IRepository repo, Guid id)
    {
        var car = repo.GetById(id);
        return car is not null ? Results.Ok(car) : Results.NotFound();
    }

    internal IResult GetAll(IRepository repo)
    {
        var cars = repo.GetAll();
        return Results.Ok(cars);
    }

    internal IResult Create(IRepository repo, Car car, IValidator<Car> validator)
    {

        var validationResult = validator.Validate(car);
        if (!validationResult.IsValid)
        {
            var errors = new { errors = validationResult.Errors.Select(x => x.ErrorMessage) };
            return Results.BadRequest(errors);
        }
        repo.Create(car);
        return Results.Created($"/cars/{car.Id}", car);
    }

    internal IResult Update(IRepository repo, Car updatedCar, Guid id)
    {
        var car = repo.GetById(id);
        if (car is null)
        {
            return Results.NotFound();
        }

        repo.Update(updatedCar);
        return Results.Ok(updatedCar);
    }

    internal IResult Delete(IRepository repo, Guid id)
    {
        repo.Delete(id);
        return Results.NoContent();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddSingleton<IRepository, DictionaryRepository>(opts => new DictionaryRepository(new Dictionary<Guid, Car>()));
    }
}
