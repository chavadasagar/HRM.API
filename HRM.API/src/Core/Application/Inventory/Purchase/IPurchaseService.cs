using HRM.API.Application.Common.Wrapper;

namespace HRM.API.Application.Inventory;
public interface IPurchaseService : ITransientService
{
    Task<long> GeneratePurchaseInvoiceId();
    Task<IResult<Guid>> CreateAsync(CreateOrUpdatePurchaseRequest request);
    Task<IResult<string>> UpdateAsync(CreateOrUpdatePurchaseRequest request);
    Task<PurchaseDto> GetPurchaseById(Guid id);
    Task<IResult<string>> DeleteAsync(Guid id);
}