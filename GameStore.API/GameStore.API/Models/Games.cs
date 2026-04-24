using GameStore.API.EndPoints;

namespace GameStore.API.Models;

public class Games
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public Genre? Genre { get; set; }

    public int GenreId { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }
    
}
