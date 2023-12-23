using System.Collections.ObjectModel;

namespace HRM.API.Shared.Catalog;
public static class HRMCountry
{
    public const string India = nameof(India);

    public static IReadOnlyList<string> Countries { get; } = new ReadOnlyCollection<string>(new[]
    {
        India
    });
}
