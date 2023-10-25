using Battleground.Models.InputModels;
using GraphQL.Types;

namespace Battleground.Api.Schema.InputTypes;

public class InventoryInputType : InputObjectGraphType<InventoryInputModel>
{
  public InventoryInputType()
  {
    Name = "InventoryInput";
    Field(i => i.PlayerId).Name("playerId").Description("ID of player.");
    Field(i => i.PokemonIdentifier).Name("pokemonIdentifier").Description("ID of pokemons");
  }
}