namespace HRM.API.Application.Catalog.Products;

public class ProductsBySearchRequestWithBrandsSpec : EntitiesByPaginationFilterSpec<Product, ProductDto>
{
    public ProductsBySearchRequestWithBrandsSpec(SearchProductsRequest request)
        : base(request) =>
        Query
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .OrderBy(c => c.Name, !request.HasOrderBy())
            .Where(p => p.BrandId.Equals(request.BrandId!.Value), request.BrandId.HasValue)
            .Where(p => p.CategoryId.Equals(request.CategoryId!.Value), request.CategoryId.HasValue)
            .Where(p => p.UnitId.Equals(request.UnitId!.Value), request.UnitId.HasValue);
            //.Where(p => p.Price >= request.MinimumRate!.Value, request.MinimumRate.HasValue)
            //.Where(p => p.Price <= request.MaximumRate!.Value, request.MaximumRate.HasValue);
}