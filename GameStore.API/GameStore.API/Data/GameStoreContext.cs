namespace GameStore.API.Data;

using GameStore.API.EndPoints;
using GameStore.API.Models; 
using Microsoft.EntityFrameworkCore;


public class GameStoreContext(DbContextOptions<GameStoreContext> options) : 
    DbContext(options)
{
    public DbSet<Games> Games => Set<Games>();

    public DbSet<Genre> Genres => Set<Genre>();
}
