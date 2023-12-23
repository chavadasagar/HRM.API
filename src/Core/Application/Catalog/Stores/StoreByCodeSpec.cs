namespace HRM.API.Application.Catalog.Stores;

public class StoreByCodeSpec : Specification<Store>, ISingleResultSpecification
{
    public StoreByCodeSpec(string code) =>
        Query.Where(b => b.Code == code);
}