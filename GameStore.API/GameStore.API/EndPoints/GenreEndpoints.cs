using GameStore.API.Data;
using GameStore.API.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.EndPoints;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this WebApplication app)
    {
        var groups = app.MapGroup("/genres");

        // Get /genres
        
        groups.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres.Select(genre => new GenreDto(genre.Id, genre.Name)).AsNoTracking().ToListAsync()
        );
    }
}
