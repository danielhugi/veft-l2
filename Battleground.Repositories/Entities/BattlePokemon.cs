namespace Battleground.Repositories.Entities;

using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(BattleId), nameof(PokemonIdentifier))]
public class BattlePokemon {
  public virtual Battle Battle { get; set; } = null!;
  public int BattleId { get; set; }
  public required string PokemonIdentifier { get; set; }
  public virtual ICollection<Attack> Attacks { get; } = new List<Attack>();
}