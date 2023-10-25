using System.Text.Json;
using Battleground.Models.Dtos;
using Battleground.Models.Exceptions;
using Battleground.Repositories.Entities;
using Battleground.Services.Interfaces;

namespace Battleground.Services.Implementations;

public class PokemonService : IPokemonService
{
  private readonly HttpClient _client;

    public PokemonService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("PokemonAPI");
    }
  public async Task<ILookup<string, Pokemon>> FetchPokemenDataFromAPI(IEnumerable<string> ids)
    {
        // Call the API to fetch Pokemon details based on the given ids...
        // Return a dictionary with id as the key and PokemonDetails as the value...

        var result = new List<Pokemon>();
        foreach (var id in ids)
        {
            var details = await GetPokemonDetailsById(id); // Replace with your actual API call
            result.Add(details);
        }
        return result.ToLookup(p => p.Name);
    }

  public async Task<IEnumerable<Pokemon>> FetchAllPokemonsFromAPI() {
    var data = await GetAllPokemons();

    return data;
  }

  public async Task<Pokemon> FetchPokemonDataFromAPI(string id) {
    return await GetPokemonDetailsById(id);
  }

  private async Task<Pokemon> GetPokemonDetailsById(string id) {
    var response = await _client.GetAsync($"pokemons/{id}"); // Replace with the correct endpoint path

    if (response.IsSuccessStatusCode)
    {
      var content = await response.Content.ReadAsStringAsync();
      var json =  JsonSerializer.Deserialize<PokemonResponse>(content);

      if (json == null) {
        throw new PokemonException($"Pokemon with identifier {id} could not be parsed.");
      }

      return json.ToPokemon();
    }

    // Handle potential errors (throw exception, return default value, etc.)
    throw new PokemonException($"Failed to fetch details for Pokemon with ID {id}. Status code: {response.StatusCode}");
  }

  private async Task<IEnumerable<Pokemon>> GetAllPokemons() {
    var response = await _client.GetAsync($"pokemons"); // Replace with the correct endpoint path

    if (response.IsSuccessStatusCode)
    {
      var content = await response.Content.ReadAsStringAsync();
      var json =  JsonSerializer.Deserialize<List<PokemonResponse>>(content);

      if (json == null) {
        throw new PokemonException($"Pokemons could not be parsed.");
      }

      return json.Select(p => p.ToPokemon());
    }

    // Handle potential errors (throw exception, return default value, etc.)
    throw new PokemonException($"Failed to fetch details for Pokemons. Status code: {response.StatusCode}");
  }
}