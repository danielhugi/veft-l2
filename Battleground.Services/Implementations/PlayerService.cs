using Battleground.Models.Exceptions;
using Battleground.Models.InputModels;
using Battleground.Repositories;
using Battleground.Repositories.Entities;
using Battleground.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battleground.Services.Implementations;

public class PlayerService : IPlayerService
{
  private readonly BattlegroundDbContext _context;

  public PlayerService(BattlegroundDbContext context)
  {
    _context = context;
  }

  public async Task<Player> AddPlayer(PlayerInputModel data) {
    var player = new Player() {
      Name = data.Name,
    };

    await _context.Players.AddAsync(player);

    await _context.SaveChangesAsync();

    return player;
  }

  public async Task<bool> RemovePlayer(int id) {
    var player = await _context.Players.FindAsync(id);

    if(player == null || player.Deleted) {
      throw new PlayerException("Player not found.");
    }

    player.Deleted = true;

    await _context.SaveChangesAsync();

    return true;
  }

  public async Task<Player> GetPlayer(int id) {
    var player = await _context.Players.FindAsync(id);

    if (player == null || player.Deleted) {
      throw new PlayerException($"Player with id {id} not found.");
    }

    return player;
  }

  public async Task<IEnumerable<Player>> GetPlayers() {
    var players = await _context.Players.ToListAsync();

    if (players == null) {
      throw new PlayerException("No players found");
    }

    return players;
  }

  public async Task<ILookup<int, Player>> GetPlayersByIds(IEnumerable<int> ids) {
    var players = await _context.Players.Where(r => ids.Contains(r.Id)).ToListAsync();

    if (players == null) {
      throw new PlayerException("No players found");
    }

    return players.ToLookup(p => p.Id);
  }
}