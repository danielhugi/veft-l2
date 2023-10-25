using Battleground.Models.Dtos;
using Battleground.Models.Exceptions;
using Battleground.Repositories;
using Battleground.Repositories.Entities;
using Battleground.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battleground.Services.Implementations;

public class InventoryService : IInventoryService
{
  private readonly BattlegroundDbContext _context;
  private readonly IPokemonService _pokemonService;

  public InventoryService(BattlegroundDbContext context, IPokemonService pokemonService)
  {
    _context = context;
    _pokemonService = pokemonService;
  }
  public async Task<bool> AddPokemonToPlayerInventory(int playerId, string pokemonIdentifier)
  {
    if (await _context.PlayerInventories.AnyAsync(i => i.PokemonIdentifier == pokemonIdentifier)) {
      throw new InventoryException($"Pokemon with identifier {pokemonIdentifier} is already in a players inventory.");
    }
    var player = await _context.Players.FindAsync(playerId);
    var pokemon = await _pokemonService.FetchPokemonDataFromAPI(pokemonIdentifier);
    if (player == null || player.Deleted) {
      throw new PlayerException($"Player with ID {playerId} not found.");
    }
    if (pokemon == null) {
      throw new PokemonException($"Pokemon with ID {pokemonIdentifier} not found.");
    }
    if (player.Inventory.Where(p => p.PokemonIdentifier == pokemon.Name).Count() > 0) {
      // Player already has pokemon in inventory, no need to have a problem.
      return false;
    }

    var playerInventory = new PlayerInventory() {
      PokemonIdentifier = pokemonIdentifier,
      AcquiredDate = new DateOnly(),
    };

    player.Inventory.Add(playerInventory);

    await _context.SaveChangesAsync();

    return true;
  }

  public async Task<bool> RemovePokemonFromPlayerInventory(int playerId, string pokemonIdentifier)
  {
    var player = await _context.Players.FindAsync(playerId);
    if (player == null || player.Deleted) {
      throw new PlayerException($"Player with ID {playerId} not found.");
    }

    var inv = player.Inventory.Where(i => i.PokemonIdentifier == pokemonIdentifier);

    if (inv.Count() == 0) {
      return false;
    }

    _context.PlayerInventories.RemoveRange(inv);

    await _context.SaveChangesAsync();

    return true;
  }
}