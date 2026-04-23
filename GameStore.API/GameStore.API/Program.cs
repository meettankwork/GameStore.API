using GameStore.API.Dtos;

const string GetGameEndpointName = "GetGame";

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


var app = builder.Build();   

// GET /games
app.MapGet("/games", () => games);

// GET /games/1
app.MapGet("/games/{id}", (int id) => games.Find(games => games.Id == id))
.WithName(GetGameEndpointName); //checking Id from over List of Games

// Post /games
app.MapPost("/games", (CreateGameDtos newGame) =>
{
    GameDto game = new(
        games.Count + 1 ,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new{id = game.Id}, game);
});

// Put /games/1
app.MapPut("/games/{id}", (int id, UpdateGameDtos updatedGame) =>
{
    var index = games.FindIndex(games => games.Id == id);

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

// Delete /games/1
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(games => games.Id == id);

    return Results.NoContent();
});

app.Run();
