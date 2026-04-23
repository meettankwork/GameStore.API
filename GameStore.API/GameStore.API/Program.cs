using GameStore.API.Dtos;

var builder = WebApplication.CreateBuilder(args);

List<GameDto> games = [
    new(
        1,
        "GTA 5", 
        "Action", 
        19.99M, 
        new DateOnly(2021, 04, 13)
    ),
    new(
        2,
        "Read Dead Redimtion II", 
        "Free World", 
        29.99M, 
        new DateOnly(2023, 08, 1)
    ),
    new(
        3,
        "Forza Horizion", 
        "Driving", 
        39.99M, 
        new DateOnly(2012, 12, 26)
    ),
    new(
        4,
        "GTA 6", 
        "Action", 
        49.99M, 
        new DateOnly(2026, 12, 30)
    ),
    new(
        5,
        "BGMI", 
        "Fighting", 
        59.99M, 
        new DateOnly(2011, 09, 11)
    )
]; 

// GET /games
builder.Build().MapGet("/games", () => games);

builder.Build().Run();
