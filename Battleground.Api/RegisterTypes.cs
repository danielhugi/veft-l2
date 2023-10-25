using Battleground.Api.Schema.Types;

public class RegisterTypes {
  public RegisterTypes(IServiceCollection services) {
    services.AddScoped<PokemonType>();
    services.AddScoped<BattleType>();
    services.AddScoped<PlayerType>();
    services.AddScoped<BattleStatusType>();
  }
}