using MasterPOS.API.Application.Catalog.Units;
using MasterPOS.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Catalog;


public class UnitsController : VersionedApiController
{
    private readonly IStringLocalizer<UnitsController> _localizer;
    public UnitsController(IStringLocalizer<UnitsController> localizer)
    {
        _localizer = localizer;
    }


    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Units)]
    [OpenApiOperation("Search units using available filters.", "")]
    public Task<PaginationResponse<UnitDto>> SearchAsync(SearchUnitsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Units)]
    [OpenApiOperation("Get unit details.", "")]
    public Task<UnitDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetUnitRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Units)]
    [OpenApiOperation("Create a new unit.", "")]
    public async Task<ActionResult> CreateAsync(CreateUnitRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["unit.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Units)]
    [OpenApiOperation("Update a unit.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateUnitRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["unit.updated"], request.Name)));
    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Units)]
    [OpenApiOperation("active/inactive a unit.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusUnitRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["unit.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Units)]
    [OpenApiOperation("Delete a unit.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeleteUnitRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["unit.deleted"], name)));
    }
}
