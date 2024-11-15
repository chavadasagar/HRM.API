using MasterPOS.API.Application.Catalog.Brands;
using MasterPOS.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Catalog;

public class BrandsController : VersionedApiController
{
    private readonly IStringLocalizer<BrandsController> _localizer;
    public BrandsController(IStringLocalizer<BrandsController> localizer)
    {
        _localizer = localizer;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Search brands using available filters.", "")]
    public Task<PaginationResponse<BrandDto>> SearchAsync(SearchBrandsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Brands)]
    [OpenApiOperation("Get brand details.", "")]
    public Task<BrandDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetBrandRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Brands)]
    [OpenApiOperation("Create a new brand.", "")]
    public async Task<ActionResult> CreateAsync(CreateBrandRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["brand.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Brands)]
    [OpenApiOperation("Update a brand.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateBrandRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["brand.updated"], request.Name)));

    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Brands)]
    [OpenApiOperation("active/inactive a brand.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusBrandRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["brand.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Brands)]
    [OpenApiOperation("Delete a brand.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeleteBrandRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["brand.deleted"], name)));
    }
}