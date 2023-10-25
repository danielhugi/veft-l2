using Battleground.Models.Exceptions;
using Battleground.Repositories.Entities;
using Battleground.Services.Interfaces;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Battleground.Api.Schema.Types;

public class PlayerType : ObjectGraphType<Player>
{
    public PlayerType(IDataLoaderContextAccessor accessor, IPokemonService pokemonService)
    {
        Name = "Player";
        Description = "A pokemon player that can take part in battles.";
        Field(d => d.Id, nullable: false).Description("The ID of the player.");
        Field(d => d.Name, nullable: false).Description("The name of the player.");
        Field<ListGraphType<BattleType>>("battles")
          .Description("What battles has this player been in.")
          .Resolve(ctx => {
            var battles = ctx.Source.Battles.Select(b => b.Battle).ToList();

            return battles;
          });
        Field<ListGraphType<PokemonType>>("inventory")
          .Description("Pokemons in inventory")
          .ResolveAsync(ctx =>
            {
              var pokemonIds = ctx.Source.Inventory.Select(p => p.PokemonIdentifier).ToList();
                // Get or add a batch loader with the key "GetUsersById"
                // The loader will call GetUsersByIdAsync for each batch of keys
              var loader = accessor.Context?.GetOrAddCollectionBatchLoader<string, Pokemon>(
                "FetchPokemenDataFromAPI", 
                async ids => await pokemonService.FetchPokemenDataFromAPI(ids)
              );

              if (loader == null) {
                throw new PlayerException("Loader not accessible.");
              }

              // Add this UserId to the pending keys to fetch
              // The execution strategy will trigger the data loader to fetch the data via GetUsersByIdAsync() at the
              //   appropriate time, and the field will be resolved with an instance of User once GetUsersByIdAsync()
              //   returns with the batched results
              return loader.LoadAsync(pokemonIds).Then(results => (object)results.SelectMany(r => r).ToList());
            });
    }
}