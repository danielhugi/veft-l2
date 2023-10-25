using Battleground.Models.Dtos;
using Battleground.Repositories.Entities;

namespace Battleground.Services.Interfaces;

public interface IPokemonService
{
  public Task<ILookup<string, Pokemon>> FetchPokemenDataFromAPI(IEnumerable<string> ids);
  public Task<Pokemon> FetchPokemonDataFromAPI(string id);
  public Task<IEnumerable<Pokemon>> FetchAllPokemonsFromAPI();
}