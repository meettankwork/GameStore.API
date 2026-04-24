using GameStore.API.EndPoints;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContex = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContex.Database.Migrate();
    }

    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStore");
//        builder.Services.AddScoped<GameStoreContext>();
        // DbContext Scope service liftime:
        // 1. It ensure new instance evrytime new pre-request
        // 2. Its connections are limited and expensive
        // 3. Makes it eay to manage transactions and ensure data consistency
        // 4. Resuing same instance can lead to increase memory usage



        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            optionsAction: options => options.UseSeeding((context,_) =>
            {
                if (!context.Set<Genre>().Any())
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Action" },
                        new Genre { Name = "Free World" },
                        new Genre { Name = "Driving" },            
                        new Genre { Name = "Fighting" }
                    );    
                    context.SaveChanges();
                }
            })
        );
    }

}
