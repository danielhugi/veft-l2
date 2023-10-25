using Battleground.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Battleground.Repositories
{
    public class BattlegroundDbContext : DbContext
    {
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Battle> Battles { get; set; }
        public virtual DbSet<BattlePlayer> BattlePlayers { get; set; }
        public virtual DbSet<BattlePokemon> BattlePokemons { get; set; }
        public virtual DbSet<PlayerInventory> PlayerInventories { get; set; }
        public virtual DbSet<Attack> Attacks { get; set; }
        public BattlegroundDbContext(DbContextOptions<BattlegroundDbContext> options) : base(options) {}
    }
}