using Battleground.Api.Schema;
using Battleground.Repositories;
using Battleground.Services.Implementations;
using Battleground.Services.Interfaces;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BattlegroundDbContext>(opt =>
        opt
        .UseLazyLoadingProxies()
        .UseNpgsql(
            builder.Configuration?.GetConnectionString("BattlegroundConnectionString"), 
            b => b.MigrationsAssembly("Battleground.Api")
        )
    );

builder.Services.AddDefer();
builder.Services.AddHttpScope();

builder.Services.AddHttpClient("PokemonAPI", client => {
    client.BaseAddress = new Uri("https://pokemon-proxy-api.herokuapp.com/");
});

builder.Services.AddTransient<IPokemonService, PokemonService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IBattleService, BattleService>();
builder.Services.AddTransient<IInventoryService, InventoryService>();

// new RegisterTypes(builder.Services);
// builder.Services.AddScoped<PokemonQuery>();
// builder.Services.AddScoped<BattlegroundSchema>();

builder.Services.AddGraphQL(qlBuilder => qlBuilder
    .AddSystemTextJson()
    .AddErrorInfoProvider(opt => opt.ExposeExceptionDetails = true)
    .AddSchema<BattlegroundSchema>(GraphQL.DI.ServiceLifetime.Scoped)
    .AddGraphTypes()
    .AddDataLoader());

var app = builder.Build();

// app.Use((context, next) =>
// {
//     using var scope = app.Services.CreateScope();
//     return next();
// });
app.UseGraphQLPlayground();
app.UseGraphQL<ISchema>();
app.MapGet("/", context =>
{
    context.Response.Redirect("/ui/playground");
    return Task.CompletedTask;
});

app.Run();
