namespace Battleground.Repositories.Entities;

using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(PokemonIdentifier), nameof(PlayerId))]
public class PlayerInventory {
  public required string PokemonIdentifier { get; set; }
  public virtual Player Player { get; set; } = null!;
  public int PlayerId { get; set; }
  public DateOnly AcquiredDate { get; set; }
}