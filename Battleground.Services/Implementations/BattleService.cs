using Battleground.Models.Enums;
using Battleground.Models.Exceptions;
using Battleground.Models.InputModels;
using Battleground.Repositories;
using Battleground.Repositories.Entities;
using Battleground.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Battleground.Services.Implementations;

public class BattleService : IBattleService
{
  private readonly BattlegroundDbContext _context;
  private readonly IPokemonService _pokemonService;
  private static readonly Random random = new Random();

  public BattleService(BattlegroundDbContext context, IPokemonService pokemonService)
  {
    _context = context;
    _pokemonService = pokemonService;
  }

    public async Task<Battle> AddBattle(BattleInputModel data)
    {
      var pokemonIds = data.PokemonIds.ToList();

      if (data.PlayerIds.Count() != 2) {
        throw new BattleException("Need two players to create a battle");
      }

      if (pokemonIds.Count != 2) {
        throw new BattleException("Need two pokemons to create a battle");
      }

      var players = await _context.Players.Where(p => data.PlayerIds.Contains(p.Id) && !p.Deleted).ToListAsync();

      if (players == null) {
        throw new PlayerException("Players not found");
      }

      if (players.Count != 2) {
        throw new BattleException("Need two players to create a battle");
      }

      if (players[0].Id == players[1].Id) {
        throw new BattleException("Player cannot battle themselves");
      }

      var pokemonFromInventory1 = players[0].Inventory.Where(i => pokemonIds[0] == i.PokemonIdentifier);
      var pokemonFromInventory2 = players[1].Inventory.Where(i => pokemonIds[1] == i.PokemonIdentifier);

      if (pokemonFromInventory1 == null) {
        throw new BattleException($"Player with ID {players[0].Id} doesn't have the pokemon with ID {pokemonIds[0]} in their inventory.");
      }

      if (pokemonFromInventory2 == null) {
        throw new BattleException($"Player with ID {players[1].Id} doesn't have the pokemon with ID {pokemonIds[1]} in their inventory.");
      }

      var battle = new Battle();

      players.ForEach(p => {
        battle.Players.Add(new BattlePlayer() {
          Player = p,
        });
      });

      pokemonIds.ForEach(p => {
        battle.Pokemons.Add(new BattlePokemon() {
          PokemonIdentifier = p,
        });
      });

      _context.Battles.Add(battle);

      await _context.SaveChangesAsync();

      return battle;
    }

    public async Task<IEnumerable<Battle>> AllBattles(BattleStatus? filter)
    {
      var battles = new List<Battle>();
      if (filter == null) {
        battles = await _context.Battles.ToListAsync();
      } else {
        battles = await _context.Battles.Where(b => b.status == filter).ToListAsync();
      }

      if (battles == null) {
        throw new BattleException("No battles found");
      }

      return battles;
    }

    public async Task<Battle> Battle(int id)
    {
      var battle = await _context.Battles.FindAsync(id);

      if (battle == null) {
        throw new BattleException("Battle not found");
      }

      return battle;
    }

    public async Task<Attack> Attack(AttackInputModel data) {
      var battle = await _context.Battles.FindAsync(data.BattleId);

      if (battle == null) {
        throw new BattleException("Battle not found");
      }

      if (battle.status == Models.Enums.BattleStatus.FINISHED) {
        throw new BattleException("Battle has concluded. No more attacks can be made");
      }

      if (battle.status == Models.Enums.BattleStatus.NOT_STARTED) {
        battle.status = Models.Enums.BattleStatus.STARTED;
      }

      var pokemons = battle.Pokemons;

      var attacker = pokemons.First(p => p.PokemonIdentifier == data.Attacker);
      var defender = pokemons.First(p => p.PokemonIdentifier != data.Attacker);

      var allAttacks = new List<Attack>();
      battle.Pokemons.ToList().ForEach(p => {
        p.Attacks.ToList().ForEach(a => {
          allAttacks.Add(a);
        });
      });

      var newestAttack = allAttacks.MaxBy(a => a.Id);
      if (newestAttack?.PokemonIdentifier == attacker.PokemonIdentifier) {
        throw new BattleException($"Pokemon with id {attacker.PokemonIdentifier} has already had their turn, {defender.PokemonIdentifier} needs to make a move first");
      }

      var attackingPlayer = battle.Players.FirstOrDefault(p => 
        p.Player.Inventory.FirstOrDefault(i => 
          i.PokemonIdentifier == attacker.PokemonIdentifier
        ) != null
      );

      if (attackingPlayer == null) {
        throw new BattleException("Attacking player doesn't own pokemon");
      }

      var pokemonsFromApi = await _pokemonService.FetchPokemenDataFromAPI(pokemons.Select(p => p.PokemonIdentifier));
      var pokeData = pokemonsFromApi.SelectMany(p => p).ToArray();
      var attackerData = pokeData.First(p => p.Name == attacker.PokemonIdentifier);
      var defenderData = pokeData.First(p => p.Name == defender.PokemonIdentifier);
      
      if (attackerData == null || attacker == null) {
        throw new BattleException("Attacker not found.");
      }
      if (defenderData == null || defender == null) {
        throw new BattleException("Defender not found.");
      }
      
      var attack = new Attack() {
        PokemonIdentifier = attacker.PokemonIdentifier,
        Success = true,
      };

      var damage = attackerData.BaseAttack;

      // Check if critical
      if (Chance(30)) {
        damage = MultiplyAndRound(damage, 1.4);
        attack.CriticalHit = true;
      }


      // Check if miss
      if (Chance(15)) {
        damage = 0;
        attack.CriticalHit = false;
        attack.Success = false;
      }

      attack.Damage = damage;


      defender.Attacks.Add(attack);
      
      var attacks = defender.Attacks;

      var defenderHealth = calculateHealth(defenderData, attacks);

      if (defenderHealth == 0) {
        battle.status = Models.Enums.BattleStatus.FINISHED;
        battle.Winner = attackingPlayer.Player;
      }

      await _context.SaveChangesAsync();

      return attack;
    }

    private int calculateHealth(Pokemon pokemon, IEnumerable<Attack> attacks) {
      var currentHealth = pokemon.HealthPoints + 0;

      attacks.ToList().ForEach(a => {
        if (a.Success && currentHealth != 0) {
          if (currentHealth < a.Damage) {
            currentHealth = 0;
          } else {
            currentHealth -= a.Damage;
          }
        }
      });

      return currentHealth;
    }

    private static bool Chance(int percent)
    {
        return random.Next(0, 100) < percent;
    }

    private static int MultiplyAndRound(int value, double multiplier)
    {
        double multipliedValue = value * multiplier;
        return (int)Math.Round(multipliedValue);
    }
}