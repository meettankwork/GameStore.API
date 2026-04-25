using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Dtos;

public record UpdateGameDtos(
    [Required][StringLength(50)] string Name, //helps to vailidate if field is empty or null  while StringLength Allow us to give max of 50 char
    [Range(1,50)] int GenreId,
    [Required][Range(10,1000)]decimal Price,
    DateOnly ReleaseDate
);
