using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Models;

namespace GameStore.API.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
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

    public static void MapGamesEndpoints(this WebApplication app)
    {

        var groups = app.MapGroup("/games");

        // GET /games
        groups.MapGet("/", () => games);

        // GET /games/1
        groups.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            ); // contidion if game exists then return game-Ok or NotFound
        })
        .WithName(GetGameEndpointName); //checking Id from over List of Games

        // Post /games
        groups.MapPost("/", async (CreateGameDtos newGame, GameStoreContext dbContext) =>
         {

            if (string.IsNullOrEmpty(newGame.Name))
            {
                return Results.BadRequest("Name is Required");
            }

            Games game = new()
            {
              Name = newGame.Name,
              GenreId = newGame.GenreId,
              Price = newGame.Price,
              ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game); // just to keep track not sql query

            await dbContext.SaveChangesAsync(); // this give sql statments

            // Bcz of security reasons
            GameDetailsDto gameDetails = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new{id = gameDetails.Id}, gameDetails);
        });

        // Put /games/1
        groups.MapPut("/{id}", (int id, UpdateGameDtos updatedGame) =>
        {
            var index = games.FindIndex(games => games.Id == id);

            // Basic Validation 
            if(index == -1)
            {
                return Results.NotFound();
            }

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
        groups.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(games => games.Id == id);

            return Results.NoContent();
        });
    }
}
