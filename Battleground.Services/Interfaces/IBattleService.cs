using Battleground.Models.Enums;
using Battleground.Models.InputModels;
using Battleground.Repositories.Entities;

namespace Battleground.Services.Interfaces;

public interface IBattleService
{
  public Task<Battle> AddBattle(BattleInputModel data);
  public Task<Battle> Battle(int id);
  public Task<IEnumerable<Battle>> AllBattles(BattleStatus? filter);
  public Task<Attack> Attack(AttackInputModel data);
}