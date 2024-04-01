using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace api.Infrastructure.MongoDB;

public sealed class Database : Domain.DbContext
{
    public override Domain.IRecipeRepository RecipeRepository { get; init; }

    public override Domain.IChefRepository ChefRepository { get; init; }

    public Database(IOptions<PersistenceSettings> persistenceSettings)
    : base()
    {
        ConventionPack camelCaseConvention = new ConventionPack {
            new CamelCaseElementNameConvention()
        };
        ConventionRegistry.Register("CamelCase", camelCaseConvention, (type) => true);

        if (!BsonClassMap.IsClassMapRegistered(typeof(RecipeDoc))) // ToDo: Check where this call belongs
        {
            BsonClassMap.RegisterClassMap<Document>(x =>
            {
                x.AutoMap();
                x.MapMember(doc => doc.ModelVersion).SetElementName("__v");
                x.MapMember(doc => doc.Id).SetElementName("_id");
            });

            BsonClassMap.RegisterClassMap<Domain.Chef>(x =>
            {
                x.AutoMap();
                x.SetDiscriminator(nameof(Document));
            });

            BsonClassMap.RegisterClassMap<RecipeDoc>(x =>
            {
                x.AutoMap();
                x.SetDiscriminator(nameof(Document));
            });
        }

        MongoClientSettings settings = MongoClientSettings.FromConnectionString(persistenceSettings.Value.ConnectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        MongoClient client = new MongoClient(settings);

        string databaseName = Globals.ApplicationNameAbbreviated.ToLower();

        IMongoDatabase db = client.GetDatabase(databaseName);

        RecipeRepository = new RecipeCollection(db);
        ChefRepository = new ChefCollection(db);
    }
}

// Help:
// http://mongodb.github.io/mongo-csharp-driver/2.2/reference/bson/mapping/
// http://mongodb.github.io/mongo-csharp-driver/2.2/reference/bson/mapping/#mapping-classes
// https://www.mongodb.com/docs/drivers/csharp/current/fundamentals/class-mapping/
