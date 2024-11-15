using HRM.API.Application.Common.Wrapper;

namespace HRM.API.Application.Inventory;
public interface IPurchaseReturnService : ITransientService
{
    Task<long> GeneratePurchaseReturnInvoiceId();
    Task<IResult<Guid>> CreateAsync(CreateOrUpdatePurchaseReturnRequest request);
    Task<IResult<string>> UpdateAsync(CreateOrUpdatePurchaseReturnRequest request);
    Task<PurchaseReturnDto> GetPurchaseReturnById(Guid id);
    Task<Result<string>> GetPurchaseReturnIdbyPurchaseInvoiceId(string id);
    Task<IResult<string>> DeleteAsync(Guid id);
}