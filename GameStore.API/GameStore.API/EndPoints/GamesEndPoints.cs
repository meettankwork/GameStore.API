using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";

    public static void MapGamesEndpoints(this WebApplication app)
    {

        var groups = app.MapGroup("/games");

        // GET /games
        groups.MapGet("/", async (GameStoreContext dbContext) 
            => await dbContext.Games
                .Include(games => games.Genre)
                .Select(games => new GameSummaryDto(
                        games.Id,
                        games.Name,
                        games.Genre!.Name,
                        games.Price,
                        games.ReleaseDate
                )).AsNoTracking().ToListAsync());

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
        groups.MapPut("/{id}", async (
            int id,
            UpdateGameDtos updatedGame,
            GameStoreContext dbContext ) =>
        {
            var excistingGame = await dbContext.Games.FindAsync(id);

            // Basic Validation 
            if(excistingGame is null)
            {
                return Results.NotFound();
            }

            excistingGame.Name = updatedGame.Name;
            excistingGame.Price = updatedGame.Price;
            excistingGame.GenreId = updatedGame.GenreId;
            excistingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // Delete /games/1
        groups.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(games => games.Id == id ).ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
