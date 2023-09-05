using RentalPropertyAPI.Data;
using Microsoft.EntityFrameworkCore;
using RentalPropertyAPI.Dtos;
using RentalPropertyAPI.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HouseDbContext>(o=> o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddCors();

builder.Services.AddScoped<IHouseRepository, HouseRepository>();

var app = builder.Build();

app.UseCors(p=> p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());

app.MapGet("/houses", (IHouseRepository repo) => repo.GetAll()).Produces<HouseDto[]>(StatusCodes.Status200OK);

app.MapGet("/house/{houseId:int}", async (int houseId, IHouseRepository repo) =>
{
    var house = await repo.GetHouse(houseId);
    if (house == null)
        return Results.Problem($"House not found with the Id {houseId}", statusCode: 404);
    else
        return Results.Ok(house);
}).ProducesProblem(404).Produces<HouseDetailDto>(StatusCodes.Status200OK);


app.MapPost("/houses", async (HouseDetailDto dto, IHouseRepository repo) =>
{
    var newHouse = await repo.AddHouse(dto);
    return Results.Created($"/house/{newHouse.Id}", newHouse);
}).Produces<HouseDetailDto>(StatusCodes.Status201Created);

app.MapPut("/houses", async (HouseDetailDto dto, IHouseRepository repo) =>
{
    if (repo.GetHouse(dto.Id) == null)
        return Results.Problem($"House not found with the Id {dto.Id}", statusCode: 404);

    var updatedHouse = await repo.UpdateHouse(dto);
    return Results.Ok(updatedHouse);

}).ProducesProblem(StatusCodes.Status404NotFound).Produces<HouseDetailDto>(StatusCodes.Status200OK);


app.MapDelete("/house/{houseId:int}", async (int houseId, IHouseRepository repo) =>
{
    if(repo.GetHouse(houseId) == null)
        return Results.Problem($"House not exists with the Id {houseId}");

    await repo.DeleteHouse(houseId);
    return Results.Ok();

}).ProducesProblem(StatusCodes.Status404NotFound).Produces(StatusCodes.Status200OK);

app.MapGet("/", () => "Hello World!");

app.Run();
