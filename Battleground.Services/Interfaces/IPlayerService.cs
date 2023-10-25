using Battleground.Models.InputModels;
using Battleground.Repositories.Entities;

namespace Battleground.Services.Interfaces;

public interface IPlayerService
{
  public Task<Player> AddPlayer(PlayerInputModel data);
  public Task<Player> GetPlayer(int id);
  public Task<bool> RemovePlayer(int id);
  public Task<IEnumerable<Player>> GetPlayers();
  public Task<ILookup<int, Player>> GetPlayersByIds(IEnumerable<int> ids);
}