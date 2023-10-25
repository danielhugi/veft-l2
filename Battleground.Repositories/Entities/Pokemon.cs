namespace Battleground.Repositories.Entities;

public class Pokemon {
  public required string Name { get; set; }
  public int HealthPoints { get; set; }
  public int BaseAttack { get; set; }
  public int Weight { get; set; }
}

#pragma warning disable IDE1006
public class PokemonResponse
{
  public required string name { get; set; }
  public int healthPoints { get; set; }
  public int baseAttack { get; set; }
  public int weight { get; set; }

  public Pokemon ToPokemon() {
    return new Pokemon() {
      Name = name,
      HealthPoints = healthPoints,
      BaseAttack = baseAttack,
      Weight = weight,
    };
  }
}