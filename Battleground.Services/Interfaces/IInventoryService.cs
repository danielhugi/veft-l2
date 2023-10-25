namespace Battleground.Services.Interfaces;

public interface IInventoryService
{
  public Task<bool> AddPokemonToPlayerInventory(int playerId, string pokemonIdentifier);
  public Task<bool> RemovePokemonFromPlayerInventory(int playerId, string pokemonIdentifier);
}