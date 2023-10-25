namespace Battleground.Repositories.Entities;

public class Attack {
  public int Id { get; set; }
  public bool Success { get; set; }
  public bool CriticalHit { get; set; }
  public int Damage { get; set; }
  public int BattlePokemonId { get; set; }
  public virtual BattlePokemon BattlePokemon { get; set; } = null!;
  public required string PokemonIdentifier { get; set; }
}