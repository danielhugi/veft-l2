namespace Battleground.Models.InputModels;

public class InventoryInputModel
{
  public required int PlayerId { get; set; }
  public required string PokemonIdentifier { get; set; }
}