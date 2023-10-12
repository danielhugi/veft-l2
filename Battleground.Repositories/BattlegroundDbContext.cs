using Microsoft.EntityFrameworkCore;

namespace Battleground.Repositories
{
    public class BattlegroundDbContext : DbContext
    {
        public BattlegroundDbContext(DbContextOptions<BattlegroundDbContext> options) : base(options) {}
    }
}