using DayThree.Cars;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;

namespace DayThree;

public static class CarsEndpoint
{
    public static void MapCarEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/car").WithTags(nameof(Car));

        group.MapGet("/", GetAllCars)
            .WithName("GetAllCars")
            .WithOpenApi();

        group.MapGet("/{id:int}", GetCarById)
            .WithName("GetCarById")
            .WithOpenApi();

        group.MapPut("/{id:int}", UpdateCar)
            .WithName("UpdateCar")
            .WithOpenApi();

        group.MapPost("/", CreateCar)
            .WithName("CreateCar")
            .WithOpenApi();

        group.MapDelete("/{id:int}", DeleteCar)
            .WithName("DeleteCar")
            .WithOpenApi();

        group.MapPost("/{id:int}/do.recharge", RechargeCar);
    }

    private static Results<Ok, NotFound, UnprocessableEntity> RechargeCar(ICarsService carsService, int id,
        RechargeCarRequest request)
    {
        var car = carsService.GetAll(x => x.Id == id).FirstOrDefault(x => x.Id == id);
        if (car is null)
        {
            return TypedResults.NotFound();
        }

        switch (car)
        {
            case ElectricCar electricCar:
                {
                    electricCar.Charge(request.timeOfRefuel);
                    break;
                }
            case FuelCar fuelCar:
                {
                    fuelCar.Refuel(request.timeOfRefuel);
                    break;
                }
            default:
                return TypedResults.UnprocessableEntity();
        }

        return TypedResults.Ok();
    }

    private static NoContent DeleteCar(ICarsService carService, int id)
    {
        carService.RemoveAll(x => x.Id == id);
        return TypedResults.NoContent();
    }

    private static Results<Created<Car>, ValidationProblem> CreateCar(ICarsService carsService, Car model,
        [FromQuery(Name = "type")] string type)
    {
        Car? car = null;
        switch (type)
        {
            case "fuel":
                car = new FuelCar
                {
                    Id = model.Id, Make = model.Make, Model = model.Model, Year = model.Year,
                };
                break;
            case "electric":
                car = new ElectricCar
                {
                    Id = model.Id, Make = model.Make, Model = model.Model, Year = model.Year,
                };
                break;
            default:
                throw new ArgumentException("Validation Problem");
                break;
        }

        carsService.Add(car!);
        return TypedResults.Created($"/api/Cars/{model.Id}", model);
    }

    private static Results<NoContent, ValidationProblem> UpdateCar(ICarsService carsService, int id, Car input)
    {
        carsService.Edit(input);
        return TypedResults.NoContent();
    }

    private static Results<Ok<Car>, NotFound> GetCarById(ICarsService carsService, int id)
    {
        var car = carsService.GetAll(x => x.Id == id).FirstOrDefault(x => x.Id == id);
        if (car is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(car);
    }

    private static Ok<Car[]> GetAllCars(ICarsService carsService)
    {
        var items = carsService.GetAll(x => true);
        return TypedResults.Ok(items.ToArray());
    }
}