using MasterPOS.API.Application.Catalog.Stores;
using MasterPOS.API.Application.Common;
using MasterPOS.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Catalog;

public class StoresController : VersionedApiController
{
    private readonly IStringLocalizer<StoresController> _localizer;
    private readonly IStoreService _storeService;
    public StoresController(IStringLocalizer<StoresController> localizer, IStoreService storeService)
    {
        (_localizer, _storeService) = (localizer, storeService);
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Stores)]
    [OpenApiOperation("Search stores using available filters.", "")]
    public Task<PaginationResponse<StoreDto>> SearchAsync(SearchStoresRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Stores)]
    [OpenApiOperation("Get store details.", "")]
    public Task<StoreDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetStoreRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Stores)]
    [OpenApiOperation("Create a new store.", "")]
    public async Task<ActionResult> CreateAsync(CreateStoreRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["store.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Stores)]
    [OpenApiOperation("Update a store.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateStoreRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["store.updated"], request.Name)));

    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Stores)]
    [OpenApiOperation("active/inactive a store.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusStoreRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["store.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Stores)]
    [OpenApiOperation("Delete a store.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeleteStoreRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["store.deleted"], name)));
    }

    //[HttpGet("generatestorecode")]
    //[OpenApiOperation("get generate store code.", "")]
    //public async Task<ActionResult> GenerateStoreCode()
    //{
    //    string storeCode = await _storeService.GenerateStoreCode();
    //    return Ok(Result<string>.SuccessAsync(storeCode, "").Result);
    //}
}
