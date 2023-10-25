using System.ComponentModel.DataAnnotations;

namespace Battleground.Models.InputModels;

public class PlayerInputModel
{
  [Required]
  public required string Name { get; set; }
}