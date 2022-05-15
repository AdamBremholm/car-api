using CarConsole.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IRepository, DictionaryRepository>(opts => new DictionaryRepository(new Dictionary<Guid, Car>()));
var app = builder.Build();

app.MapGet("/cars", ([FromServices] IRepository repo) =>
    {
        var cars = repo.GetAll();
        return Results.Ok(cars);
    });
app.MapGet("/cars/{id}", ([FromServices] IRepository repo, Guid id) =>
    {
        var car = repo.GetById(id);
        return car is not null ? Results.Ok(car) : Results.NotFound();
    });
app.MapPost("/cars", (Car car, [FromServices] IRepository repo) =>
    {
        repo.Create(car);
        return Results.Created($"/cars/{car.Id}", car);
    });
app.MapPut("/cars/{id}", (Car updatedCar, [FromServices] IRepository repo, Guid id) =>
    {
        var car = repo.GetById(id);
        if (car is null)
        {
            return Results.NotFound();
        }

        repo.Update(updatedCar);
        return Results.Ok(updatedCar);

    });
app.MapDelete("/cars/{id}", ([FromServices] IRepository repo, Guid id) => 
    {
    repo.Delete(id);
    });
app.Run();
