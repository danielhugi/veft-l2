namespace Battleground.Repositories.Entities;

public class PokemonDetails {
    public required string Name { get; set; }
    public int HealthPoints { get; set; }
    public int BaseAttack { get; set; }
    public int Weight { get; set; }
}