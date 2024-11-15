namespace HRM.API.Application.Catalog.Stores;

public class StoreByNameSpec : Specification<Store>, ISingleResultSpecification
{
    public StoreByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}