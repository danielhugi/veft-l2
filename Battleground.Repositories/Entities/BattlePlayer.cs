namespace Battleground.Repositories.Entities;

using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(BattleId), nameof(PlayerId))]
public class BattlePlayer {
  public virtual Battle Battle { get; set; } = null!;
  public int BattleId { get; set; }
  public virtual Player Player { get; set; } = null!;
  public int PlayerId { get; set; }
  
}