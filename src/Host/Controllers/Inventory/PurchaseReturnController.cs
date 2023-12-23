using HRM.API.Application.Common.Wrapper;
using HRM.API.Application.Inventory;
using Microsoft.Extensions.Localization;

namespace HRM.API.Host.Controllers.Inventory;

public class PurchaseReturnController : VersionedApiController
{
    private readonly IStringLocalizer<PurchaseReturnController> _localizer;
    private readonly IPurchaseReturnService _purchaseReturnService;
    public PurchaseReturnController(IStringLocalizer<PurchaseReturnController> localizer, IPurchaseReturnService purchaseReturnService)
    {
        _localizer = localizer;
        _purchaseReturnService = purchaseReturnService;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.PurchaseReturn)]
    [OpenApiOperation("Search Purchase Return using available filters.", "")]
    public Task<PaginationResponse<PurchaseReturnDto>> SearchAsync(SearchPurchasesReturnRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.PurchaseReturn)]
    [OpenApiOperation("Get Purchase Return details.", "")]
    public Task<PurchaseReturnDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetPurchaseReturnRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.PurchaseReturn)]
    [OpenApiOperation("Create a new Purchase Return.", "")]
    public async Task<ActionResult> CreateAsync(CreateOrUpdatePurchaseReturnRequest request)
    {
        var result = await _purchaseReturnService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.PurchaseReturn)]
    [OpenApiOperation("Update a purchase return.", "")]
    public async Task<ActionResult> UpdateAsync(CreateOrUpdatePurchaseReturnRequest request, Guid id)
    {
        if (id == null && id != request.Id)
            return BadRequest();

        var result = await _purchaseReturnService.UpdateAsync(request);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.PurchaseReturn)]
    [OpenApiOperation("Delete a Purchase Return.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _purchaseReturnService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpDelete("payment/{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.PurchaseReturn)]
    [OpenApiOperation("Delete a Purchase Return Payment.", "")]
    public async Task<ActionResult> DeletePaymentAsync(Guid id)
    {
        await Mediator.Send(new DeletePurchaseReturnPaymentRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["purchasereturn.deletedpayment"]));
    }

    [HttpGet("GetPurchaseReturnIdByInvoiceId/{id}")]
    [MustHavePermission(FSHAction.View, FSHResource.Purchases)]
    [OpenApiOperation("Get a Purchase Return Id by Purchase Invoice Id.", "")]
    public async Task<ActionResult> GetPurchaseReturnIdByInvoiceId(string id)
    {
        var result = await _purchaseReturnService.GetPurchaseReturnIdbyPurchaseInvoiceId(id);
        return Ok(result);
    }
}
