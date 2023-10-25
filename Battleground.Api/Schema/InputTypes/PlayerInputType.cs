using Battleground.Models.InputModels;
using GraphQL.Types;

namespace Battleground.Api.Schema.InputTypes;

public class PlayerInputType : InputObjectGraphType<PlayerInputModel>
{
  public PlayerInputType()
  {
    Name = "PlayerInput";
    Field(p => p.Name).Name("name");
  }
}