using MasterPOS.API.Application.Inventory;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Inventory;

public class InventoryController : VersionedApiController
{
    private readonly IStringLocalizer<InventoryController> _localizer;
    private readonly IInventoryService _inventoryService;
    public InventoryController(IStringLocalizer<InventoryController> localizer, IInventoryService inventoryService)
    {
        _localizer = localizer;
        _inventoryService = inventoryService;
    }

    [HttpPost("getproductstock")]
    [OpenApiOperation("Get Product stock.", "")]
    public async Task<ActionResult> GetProductStock(ProductStockRequest request)
    {
        decimal result = await _inventoryService.GetProductStock(request.ProductId, request.StoreId);
        return Ok(result);
    }

    [HttpPost("getproductstockbypurchasereturn")]
    [OpenApiOperation("Get Product stock.", "")]
    public async Task<ActionResult> GetProductStockByPurchaseReturn(ProductStockRequest request)
    {
        decimal result = await _inventoryService.GetProductStock(request.ProductId, request.StoreId, request.TypeId ?? Guid.Empty);
        return Ok(result);
    }
}
