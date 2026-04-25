using GameStore.API.Data;
using GameStore.API.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();   

app.MapGamesEndpoints();
app.MapGenreEndpoints();

app.MigrateDb();

app.Run();
