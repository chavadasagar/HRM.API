namespace MasterPOS.API.Application.Catalog;
public class ProductsByCategorySpec : Specification<Product>
{
    public ProductsByCategorySpec(Guid categoryId) =>
      Query.Where(p => p.CategoryId == categoryId);
}
