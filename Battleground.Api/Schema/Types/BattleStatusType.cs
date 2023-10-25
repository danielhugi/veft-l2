using Battleground.Models.Enums;
using GraphQL.Types;

namespace Battleground.Api.Schema.Types;

public class BattleStatusType : EnumerationGraphType<BattleStatus>
{
    public BattleStatusType()
    {
        Name = "BattleStatusType";
        Description = "Status of the battle.";
        Add("NOT_STARTED", BattleStatus.NOT_STARTED, "Battle has not started.");
        Add("STARTED", BattleStatus.STARTED, "Battle has been started.");
        Add("FINISHED", BattleStatus.FINISHED, "Battle has finished and a winner has been chosen.");
    }
}