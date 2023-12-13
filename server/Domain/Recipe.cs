namespace api.Domain;

public sealed class Recipe(EntityId entityId) : Entity(entityId)
{
    public const int MODEL_VERSION = 0;

    public override int ModelVersion { get; init; } = MODEL_VERSION;

    public required string Name { get; set; }
}
