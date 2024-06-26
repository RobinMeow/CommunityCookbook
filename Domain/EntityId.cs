namespace Domain;

public sealed record EntityId
{
    static readonly IdentifierSpecification[] s_identifierSpecifications = [
        new GuidEntityIdSpecification(),
    ];

    readonly string _id = "00000000-0000-0000-0000-000000000000";

    public string Id { get => _id; }

    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null or white space.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is not in the correct format or is a disallowed ID.</exception>
    public EntityId(string id)
    {
        bool validId = false;
        for (int i = 0; i < s_identifierSpecifications.Length; i++)
        {
            if (s_identifierSpecifications[i].IsSatisfiedBy(id))
            {
                validId = true;
                break;
            }
        }

        if (!validId)
        {
            throw new ArgumentException($"Id '{id}' does not satisfy any IdentifierSpecification.");
        }

        _id = id.ToLower();
    }

    /// <summary>Generates a new valid entity ID by generating a new GUID string until a non-disallowed ID is found.</summary>
    /// <returns>A new <see cref="EntityId"/> instance.</returns>
    public static EntityId New()
    {
        string newId;

        do
            newId = Guid.NewGuid().ToString(GuidEntityIdSpecification.GuidFormat).ToLower();
        while (GuidEntityIdSpecification.IsDisallowedId(newId));

        return new EntityId(newId);
    }

    public override string ToString()
    {
        return Id;
    }
}
