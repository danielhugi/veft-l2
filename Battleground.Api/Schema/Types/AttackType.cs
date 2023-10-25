using Battleground.Models.Exceptions;
using Battleground.Repositories.Entities;
using Battleground.Services.Interfaces;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Battleground.Api.Schema.Types;

public class AttackType : ObjectGraphType<Attack>
{
    public AttackType(IDataLoaderContextAccessor accessor, IPokemonService pokemonService)
    {
      Name = "Attack";
      Description = "An attack from a pokemon in a battle";

      Field<IntGraphType>("damageDealt")
        .Description("Damage dealt")
        .Resolve(ctx => ctx.Source.Damage);

      Field<BooleanGraphType>("criticalHit")
        .Description("Was the attack a critical hit?")
        .Resolve(ctx => ctx.Source.CriticalHit);

      Field<BooleanGraphType>("successfulHit")
        .Description("Was the attack a success?")
        .Resolve(ctx => ctx.Source.Success);

      Field<PokemonType>("attackedBy")
        .Description("Was the attack a success?")
        .ResolveAsync(ctx =>
        {
          var pokemonId = ctx.Source.PokemonIdentifier;
            // Get or add a batch loader with the key "GetUsersById"
            // The loader will call GetUsersByIdAsync for each batch of keys
          var loader = accessor.Context?.GetOrAddCollectionBatchLoader<string, Pokemon>(
            "FetchPokemenDataFromAPI", 
            async ids => await pokemonService.FetchPokemenDataFromAPI(ids)
          );

          if (loader == null) {
            throw new BattleException("Loader not accessible.");
          }

          // Add this UserId to the pending keys to fetch
          // The execution strategy will trigger the data loader to fetch the data via GetUsersByIdAsync() at the
          //   appropriate time, and the field will be resolved with an instance of User once GetUsersByIdAsync()
          //   returns with the batched results
          return loader.LoadAsync(pokemonId).Then(res => (object)res.First(p => p.Name == pokemonId));
        });
    }
}