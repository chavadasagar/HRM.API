using MasterPOS.API.Application.Catalog.Suppliers;
using MasterPOS.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Catalog;

public class SuppliersController : VersionedApiController
{
    private readonly IStringLocalizer<SuppliersController> _localizer;
    public SuppliersController(IStringLocalizer<SuppliersController> localizer)
    {
        (_localizer) = (localizer);
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Suppliers)]
    [OpenApiOperation("Search Suppliers using available filters.", "")]
    public Task<PaginationResponse<SupplierDto>> SearchAsync(SearchSuppliersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Suppliers)]
    [OpenApiOperation("Get Supplier details.", "")]
    public Task<SupplierDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetSupplierRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Suppliers)]
    [OpenApiOperation("Create a new Supplier.", "")]
    public async Task<ActionResult> CreateAsync(CreateSupplierRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["supplier.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Suppliers)]
    [OpenApiOperation("Update a Supplier.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateSupplierRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["supplier.updated"], request.Name)));

    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Suppliers)]
    [OpenApiOperation("active/inactive a Supplier.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusSupplierRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["supplier.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Suppliers)]
    [OpenApiOperation("Delete a Supplier.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeleteSupplierRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["supplier.deleted"], name)));
    }
}
