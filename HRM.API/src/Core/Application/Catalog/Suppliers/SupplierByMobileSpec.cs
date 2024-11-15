namespace MasterPOS.API.Application.Catalog.Suppliers;
public class SupplierByMobileSpec : Specification<Supplier>, ISingleResultSpecification
{
    public SupplierByMobileSpec(string mobile) =>
        Query.Where(b => b.Mobile == mobile);
}