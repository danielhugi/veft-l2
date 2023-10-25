namespace Battleground.Models.InputModels;

public class BattleInputModel
{
  public required IEnumerable<int> PlayerIds { get; set; }
  public required IEnumerable<string> PokemonIds { get; set; }
}