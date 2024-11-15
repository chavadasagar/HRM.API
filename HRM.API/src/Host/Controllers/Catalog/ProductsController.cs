using HRM.API.Application.Catalog.Products;
using HRM.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace HRM.API.Host.Controllers.Catalog;


public class ProductsController : VersionedApiController
{
    private readonly IStringLocalizer<ProductsController> _localizer;
    public ProductsController(IStringLocalizer<ProductsController> localizer)
    {
        _localizer = localizer;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Products)]
    [OpenApiOperation("Search products using available filters.", "")]
    public Task<PaginationResponse<ProductDto>> SearchAsync(SearchProductsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Products)]
    [OpenApiOperation("Get product details.", "")]
    public Task<ProductDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetProductRequest(id));
    }

    //[HttpGet("dapper")]
    //[MustHavePermission(FSHAction.View, FSHResource.Products)]
    //[OpenApiOperation("Get product details via dapper.", "")]
    //public Task<ProductDto> GetDapperAsync(Guid id)
    //{
    //    return Mediator.Send(new GetProductViaDapperRequest(id));
    //}

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Products)]
    [OpenApiOperation("Create a new product.", "")]
    public async Task<ActionResult> CreateAsync(CreateProductRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["product.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Products)]
    [OpenApiOperation("Update a product.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateProductRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["product.updated"], request.Name)));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Products)]
    [OpenApiOperation("Delete a product.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeleteProductRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["product.deleted"], name)));
    }


    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Products)]
    [OpenApiOperation("active/inactive a product.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusProductRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["product.updatedstatus"]));
    }

    //[HttpPost("export")]
    //[MustHavePermission(FSHAction.Export, FSHResource.Products)]
    //[OpenApiOperation("Export a products.", "")]
    //public async Task<FileResult> ExportAsync(ExportProductsRequest filter)
    //{
    //    var result = await Mediator.Send(filter);
    //    return File(result, "application/octet-stream", "ProductExports");
    //}
}