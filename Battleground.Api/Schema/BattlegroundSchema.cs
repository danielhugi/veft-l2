using GraphQL.Instrumentation;

namespace Battleground.Api.Schema;

public class BattlegroundSchema : GraphQL.Types.Schema
{
    public BattlegroundSchema(IServiceProvider provider)
        : base(provider)
    {
        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}