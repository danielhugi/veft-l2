using Battleground.Api.Schema.InputTypes;
using Battleground.Api.Schema.Types;
using Battleground.Models.InputModels;
using Battleground.Services.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace Battleground.Api.Schema.Mutations;

public class Mutation : ObjectGraphType
{
  private readonly IPlayerService _playerService;
  private readonly IBattleService _battleService;
  private readonly IInventoryService _inventoryService;


    public Mutation(IPlayerService playerService, IBattleService battleService, IInventoryService inventoryService)
    {
      _playerService = playerService;
      _battleService = battleService;
      _inventoryService = inventoryService;


      Field<PlayerType>("addPlayer")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<PlayerInputType>> { Name = "input" }))
        .ResolveAsync(
          async ctx => {
            var data = ctx.GetArgument<PlayerInputModel>("input");
            return await _playerService.AddPlayer(data);
          }
        );

      Field<PlayerType>("removePlayer")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }))
        .ResolveAsync(
          async ctx => {
            var data = ctx.GetArgument<int>("id");
            return await _playerService.RemovePlayer(data);
          }
        );

      Field<BattleType>("addBattle")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<BattleInputType>> { Name = "input" }))
        .ResolveAsync(
          async ctx => {
            var data = ctx.GetArgument<BattleInputModel>("input");
            return await _battleService.AddBattle(data);
          }
        );

      Field<BooleanGraphType>("addPokemonToInventory")
      .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<InventoryInputType>> { Name = "input" }))
      .ResolveAsync(
        async ctx => {
          var data = ctx.GetArgument<InventoryInputModel>("input");
          return await _inventoryService.AddPokemonToPlayerInventory(data.PlayerId, data.PokemonIdentifier);
        }
      );

      Field<BooleanGraphType>("removePokemonFromInventory")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<InventoryInputType>> { Name = "input" }))
        .ResolveAsync(
          async ctx => {
            var data = ctx.GetArgument<InventoryInputModel>("input");
            return await _inventoryService.AddPokemonToPlayerInventory(data.PlayerId, data.PokemonIdentifier);
          }
        );

      Field<AttackType>("attack")
        .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<AttackInputType>> { Name = "input" }))
        .ResolveAsync(
          async ctx => {
            var data = ctx.GetArgument<AttackInputModel>("input");
            return await _battleService.Attack(data);
          }
        );
    }
}