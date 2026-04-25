namespace GameStore.API.Dtos;

// DTO (Data Transfer Object) is a design pattern used to 
// transfer data between different parts of a system

public record GameSummaryDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
