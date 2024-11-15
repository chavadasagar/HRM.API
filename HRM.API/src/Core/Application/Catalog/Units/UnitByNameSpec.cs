using Unit = HRM.API.Domain.Catalog.Unit;

namespace HRM.API.Application.Catalog.Units;
public class UnitByNameSpec : Specification<Unit>, ISingleResultSpecification
{
    public UnitByNameSpec(string name) =>
      Query.Where(b => b.Name == name);
}
