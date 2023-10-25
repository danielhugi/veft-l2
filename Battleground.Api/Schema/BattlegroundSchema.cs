using Battleground.Api.Schema.Mutations;
using Battleground.Api.Schema.Queries;
using GraphQL.Instrumentation;

namespace Battleground.Api.Schema;

public class BattlegroundSchema : GraphQL.Types.Schema
{
    public BattlegroundSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = provider.GetRequiredService<Query>();

        Mutation = provider.GetRequiredService<Mutation>();

        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}