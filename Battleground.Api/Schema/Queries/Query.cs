using Battleground.Api.Schema.Types;
using Battleground.Models.Enums;
using Battleground.Services.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace Battleground.Api.Schema.Queries;

public class Query : ObjectGraphType
{
  private readonly IPlayerService _playerService;
  private readonly IPokemonService _pokemonService;
  private readonly IBattleService _battleService;

    public Query(IPlayerService playerService, IPokemonService pokemonService, IBattleService battleService)
    {
        _playerService = playerService;
        _pokemonService = pokemonService;
        _battleService = battleService;

        Field<PlayerType>("player")
          .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "ID of the Player" }))
          .ResolveAsync(
            async ctx => {
              var id = ctx.GetArgument<int>("id");
              return await _playerService.GetPlayer(id);
            }
          );

        Field<ListGraphType<PlayerType>>("allPlayers")
          .ResolveAsync(
            async ctx => {
              return await _playerService.GetPlayers();
            }
          );

        Field<PokemonType>("pokemon")
          .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "ID of the Pokemon" }))
          .ResolveAsync(
            async ctx => {
              var id = ctx.GetArgument<string>("id");
              return await _pokemonService.FetchPokemonDataFromAPI(id); // Assuming you have a method to get a Pokémon by its ID
            }
          );

        Field<ListGraphType<PokemonType>>("allPokemons")
          .ResolveAsync(
            async ctx => {
              return await _pokemonService.FetchAllPokemonsFromAPI();
            }
          );

        Field<ListGraphType<BattleType>>("allBattles")
          .Arguments(new QueryArguments(new QueryArgument<BattleStatusType> { Name = "status", Description = "Filter by status of battle" }))
          .ResolveAsync(
            async ctx => {
              var status = ctx.GetArgument<BattleStatus?>("status");
              return await _battleService.AllBattles(status);
            }
          );

        Field<BattleType>("battle")
          .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "ID of the Battle" }))
          .ResolveAsync(
            async ctx => {
              var id = ctx.GetArgument<int>("id");
              return await _battleService.Battle(id);
            }
          );
    }
}