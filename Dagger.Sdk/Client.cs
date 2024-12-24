namespace Dagger.Sdk;

public static class Client
{
    readonly static Lazy<Query> LazyQuery;
    
    static Client()
    {
        LazyQuery = new Lazy<Query>(() => new Query(QueryBuilder.Create(), new GraphQLClient()));
    }
    
    public static Query Query() => LazyQuery.Value;
}