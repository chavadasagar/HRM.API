namespace MasterPOS.API.Application.Catalog;
public class ProductsByUnitSpec : Specification<Product>
{
    public ProductsByUnitSpec(Guid unitId) =>
      Query.Where(p => p.UnitId == unitId);
}
