namespace Battleground.Models.Exceptions;

public class BattleException : Exception
{
    public BattleException(string message) : base(message) {}
}