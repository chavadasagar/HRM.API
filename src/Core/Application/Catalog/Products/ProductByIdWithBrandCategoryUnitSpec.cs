namespace HRM.API.Application.Catalog.Products;

public class ProductByIdWithBrandCategoryUnitSpec : Specification<Product, ProductDetailsDto>, ISingleResultSpecification
{
    public ProductByIdWithBrandCategoryUnitSpec(Guid id) =>
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Brand)
            .Include(p => p.Category);
}