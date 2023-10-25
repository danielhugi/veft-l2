using Battleground.Models.Enums;

namespace Battleground.Repositories.Entities;

public class Battle {
  public int Id { get; set; }
  public virtual ICollection<BattlePlayer> Players { get; } = new List<BattlePlayer>();
  public virtual Player? Winner { get; set; }
  public int? WinnerId { get; set; }
  public virtual ICollection<BattlePokemon> Pokemons { get; } = new List<BattlePokemon>();
  public BattleStatus status { get; set; } = BattleStatus.NOT_STARTED;
}