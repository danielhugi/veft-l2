namespace Battleground.Repositories.Entities;

public class Player {
  public int Id { get; set; }
  public required string Name { get; set; }
  public bool Deleted { get; set; } = false;
  public virtual ICollection<BattlePlayer> Battles { get; } = new List<BattlePlayer>();
  public virtual ICollection<PlayerInventory> Inventory { get; } = new List<PlayerInventory>();
};
