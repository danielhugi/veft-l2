using Battleground.Models.InputModels;
using GraphQL.Types;

namespace Battleground.Api.Schema.InputTypes;

public class BattleInputType : InputObjectGraphType<BattleInputModel>
{
  public BattleInputType()
  {
    Name = "BattleInput";
    Field(p => p.PlayerIds).Name("playerIds").Description("IDs of players in the battle.");
    Field(p => p.PokemonIds).Name("pokemonIds").Description("IDs of pokemons in inventory of players.");
  }
}