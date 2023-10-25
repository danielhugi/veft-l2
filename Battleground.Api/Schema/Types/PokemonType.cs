using Battleground.Repositories.Entities;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Battleground.Api.Schema.Types;

public class PokemonType : ObjectGraphType<Pokemon>
{
    public PokemonType(IDataLoaderContextAccessor dataLoaderAccessor)
    {
        Name = "Pokemon";
        Description = "A pokemon";
        
        Field(p => p.Name)
          .Name("name");

        Field(p => p.HealthPoints)
          .Name("healthPoints");

        Field(p => p.BaseAttack)
          .Name("baseAttack");

        Field(p => p.Weight).Name("weight");
    }
}