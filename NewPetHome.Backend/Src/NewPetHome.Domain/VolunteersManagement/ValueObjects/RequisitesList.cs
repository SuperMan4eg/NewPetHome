using NewPetHome.Domain.Shared.ValueObjects;

namespace NewPetHome.Domain.VolunteersManagement.ValueObjects;

public record RequisitesList
{
    private RequisitesList()
    {
    }

    public RequisitesList(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }

    public IReadOnlyList<Requisite> Requisites { get; } = [];
}