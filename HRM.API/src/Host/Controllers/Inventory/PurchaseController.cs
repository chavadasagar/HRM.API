using MasterPOS.API.Application.Common.Wrapper;
using MasterPOS.API.Application.Inventory;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Inventory;

public class PurchaseController : VersionedApiController
{
    private readonly IStringLocalizer<PurchaseController> _localizer;
    private readonly IPurchaseService _purchaseService;
    public PurchaseController(IStringLocalizer<PurchaseController> localizer, IPurchaseService purchaseService)
    {
        _localizer = localizer;
        _purchaseService = purchaseService;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Purchases)]
    [OpenApiOperation("Search Purchases using available filters.", "")]
    public Task<PaginationResponse<PurchaseDto>> SearchAsync(SearchPurchasesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Purchases)]
    [OpenApiOperation("Get Purchase details.", "")]
    public Task<PurchaseDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetPurchaseRequest(id));
    }

    [HttpGet("getpurchasebyinvoiceid/{id}")]
    [MustHavePermission(FSHAction.View, FSHResource.Purchases)]
    [OpenApiOperation("Get Purchase details by Invoice Id.", "")]
    public Task<PurchaseDto> GetPurchaseByInvoiceIdAsync(string id)
    {
        return Mediator.Send(new GetPurchaseByInvoiceIdRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Purchases)]
    [OpenApiOperation("Create a new Purchase.", "")]
    public async Task<ActionResult> CreateAsync(CreateOrUpdatePurchaseRequest request)
    {
        var result = await _purchaseService.CreateAsync(request);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Purchases)]
    [OpenApiOperation("Update a purchase.", "")]
    public async Task<ActionResult> UpdateAsync(CreateOrUpdatePurchaseRequest request, Guid id)
    {
        if (id == null && id != request.Id)
            return BadRequest();

        var result = await _purchaseService.UpdateAsync(request);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Purchases)]
    [OpenApiOperation("Delete a Purchase.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var result = await _purchaseService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpDelete("payment/{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Purchases)]
    [OpenApiOperation("Delete a Purchase Payment.", "")]
    public async Task<ActionResult> DeletePaymentAsync(Guid id)
    {
        await Mediator.Send(new DeletePurchasePaymentRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["purchase.deletedpayment"]));
    }
}
