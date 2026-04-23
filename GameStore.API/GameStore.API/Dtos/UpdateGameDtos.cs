namespace GameStore.API.Dtos;

public record UpdateGameDtos(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
