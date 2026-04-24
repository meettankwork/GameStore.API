using GameStore.API.Data;
using GameStore.API.EndPoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();   

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
