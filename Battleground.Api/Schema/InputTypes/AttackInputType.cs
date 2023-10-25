using Battleground.Models.InputModels;
using GraphQL.Types;

namespace Battleground.Api.Schema.InputTypes;

public class AttackInputType : InputObjectGraphType<AttackInputModel>
{
  public AttackInputType()
  {
    Name = "AttackInput";
    Field(p => p.Attacker).Name("attacker").Description("ID of attacking pokemon.");
    Field(p => p.BattleId).Name("battleId").Description("ID of battle.");
  }
}