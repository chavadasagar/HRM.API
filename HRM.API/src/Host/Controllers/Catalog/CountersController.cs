using MasterPOS.API.Application.Catalog.Counters;
using MasterPOS.API.Application.Catalog.Stores;
using MasterPOS.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Catalog;

public class CountersController : VersionedApiController
{
    private readonly IStringLocalizer<CountersController> _localizer;
    public CountersController(IStringLocalizer<CountersController> localizer)
    {
        _localizer = localizer;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Counters)]
    [OpenApiOperation("Search counters using available filters.", "")]
    public async Task<PaginationResponse<CounterDto>> SearchAsync(SearchCountersRequest request)
    {
        return await Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Counters)]
    [OpenApiOperation("Get counter details.", "")]
    public Task<CounterDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetCounterRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Counters)]
    [OpenApiOperation("Create a new counter.", "")]
    public async Task<ActionResult> CreateAsync(CreateCounterRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["counter.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Counters)]
    [OpenApiOperation("Update a counter.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateCounterRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["counter.updated"], request.Name)));
    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Counters)]
    [OpenApiOperation("active/inactive a counter.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusCounterRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["counter.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Counters)]
    [OpenApiOperation("Delete a counter.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteCounterRequest(id));
    }
}
