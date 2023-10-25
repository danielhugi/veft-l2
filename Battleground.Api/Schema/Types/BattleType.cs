using Battleground.Models.Exceptions;
using Battleground.Repositories.Entities;
using Battleground.Services.Interfaces;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Battleground.Api.Schema.Types;

public class BattleType : ObjectGraphType<Battle>
{
    public BattleType(IDataLoaderContextAccessor accessor, IPokemonService pokemonService)
    {
      Name = "Battle";
      Description = "A Pokemon battle between players.";
      
      Field(d => d.Id)
        .Name("id")
        .Description("The ID of the battle.");

      Field<PlayerType, Player>("winner")
        .Description("The winning player of the battle.")
        .Resolve(ctx => ctx.Source.Winner);

      Field<ListGraphType<PlayerType>>("players")
        .Description("The players in the battle.")
        .Resolve(ctx => ctx.Source.Players.Select(p => p.Player));

      Field<ListGraphType<AttackType>>("attacks")
        .Description("The attacks of the battle.")
        .Resolve(ctx => {
          var attacks = new List<Attack>();
          ctx.Source.Pokemons.ToList().ForEach(p => {
            p.Attacks.ToList().ForEach(a => {
              attacks.Add(a);
            });
          });
          return attacks;
        });

      Field<ListGraphType<PokemonType>>("pokemons")
        .Description("The Pokemon participating in the battle.")
        .ResolveAsync(ctx =>
        {
          var pokemonIds = ctx.Source.Pokemons.Select(p => p.PokemonIdentifier).ToList();
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
          return loader.LoadAsync(pokemonIds).Then(results => (object)results.SelectMany(r => r).ToList());
        });

      Field(b => b.status)
        .Description("The status of the battle.")
        .Name("status");
    }
}