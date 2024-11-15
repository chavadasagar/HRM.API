using Finbuckle.MultiTenant;
using MasterPOS.API.Application.Common.Events;
using MasterPOS.API.Application.Common.Exceptions;
using MasterPOS.API.Application.Common.Interfaces;
using MasterPOS.API.Application.Common.Wrapper;
using MasterPOS.API.Application.Inventory;
using MasterPOS.API.Domain.Common;
using MasterPOS.API.Domain.Inventory;
using MasterPOS.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Infrastructure.Inventory;
public class PurchaseReturnService : IPurchaseReturnService
{
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<PurchaseReturnService> _localizer;
    private readonly ICurrentUser _currentUser;
    private readonly ITenantInfo _currentTenant;
    private readonly IEventPublisher _events;

    public PurchaseReturnService(
        ApplicationDbContext context,
        IStringLocalizer<PurchaseReturnService> localizer,
        ICurrentUser currentUser,
        ITenantInfo currentTenant,
        IEventPublisher events)
    {
        _context = context;
        _localizer = localizer;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _events = events;
    }

    public async Task<IResult<Guid>> CreateAsync(CreateOrUpdatePurchaseReturnRequest request)
    {
        Guid purchaseReturnId = Guid.Empty;
        long pRIId = await GeneratePurchaseReturnInvoiceId();

        string purchaseReturnInvoiceId = string.Empty;
        if (pRIId >= 999)
            purchaseReturnInvoiceId = Convert.ToString(pRIId);
        else
            purchaseReturnInvoiceId = Convert.ToString(pRIId).PadLeft(4, '0');

        purchaseReturnInvoiceId = string.Format("{0}{1}", GlobalConstant.PrefixPurchaseReturnInvoice, purchaseReturnInvoiceId);

        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var purchase = new PurchaseReturn(pRIId, purchaseReturnInvoiceId: purchaseReturnInvoiceId, purchaseInvoiceId: request.PurchaseInvoiceId, referenceNo: request.ReferenceNo, storeId: request.StoreId, supplierId: request.SupplierId, purchaseReturnDate: DateTime.Parse(request.PurchaseReturnDate), purchaseReturnStatus: request.PurchaseReturnStatus, totalQuantity: request.TotalQuantity, cGST: request.CGST, sGST: request.SGST, totalTax: request.TotalTax, otherCharge: request.OtherCharge, discount: request.Discount, discountType: request.DiscountType, totalDiscount: request.TotalDiscount, subtotalAmount: request.SubtotalAmount, totalAmount: request.TotalAmount, note: request.Note);
                await _context.PurchaseReturns.AddAsync(purchase);
                int result = await _context.SaveChangesAsync();
                if (result < 0)
                {
                    transaction.Rollback();
                }

                purchaseReturnId = purchase.Id;
                if (purchase.Id != Guid.Empty)
                {
                    foreach (PurchaseReturnProductRequest pp in request.PurchaseReturnProducts)
                    {
                        var purduct = new PurchaseReturnProduct(purchaseReturnId: purchaseReturnId, productId: pp.ProductId, productName: pp.ProductName, quantity: pp.Quantity, purchasePrice: pp.PurchasePrice, discount: pp.Discount, discountType: pp.DiscountType, totalDiscount: pp.TotalDiscount, cGST: pp.CGST, sGST: pp.SGST, taxType: pp.TaxType, totalTax: pp.TotalTax, unitCostAfterTaxAndDiscount: pp.UnitCostAfterTaxAndDiscount, totalAmount: pp.TotalAmount);
                        await _context.PurchaseReturnProducts.AddAsync(purduct);

                        var purductQuantity = new ProductQuantiy(productId: pp.ProductId, storeId: request.StoreId, quantity: pp.Quantity, typeId: purchaseReturnId, type: (short)ProductType.PurchaseReturn);
                        await _context.ProductQuantities.AddAsync(purductQuantity);
                    }

                    if (request.PurchaseReturnPayments != null)
                    {
                        var payment = new PurchaseReturnPayment(purchaseReturnId: purchaseReturnId, supplierId: purchase.SupplierId, paymentTypeId: request.PurchaseReturnPayments.PaymentTypeId, amount: request.PurchaseReturnPayments.Amount, paymentDate: DateTime.Now, note: request.PurchaseReturnPayments.Note);
                        await _context.PurchaseReturnPayments.AddAsync(payment);
                    }
                }

                await _context.SaveChangesAsync();
                transaction.Commit();
                transaction.Dispose();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction.Dispose();
                throw;
            }
        }

        return await Result<Guid>.SuccessAsync(purchaseReturnId, string.Format(_localizer["purchasereturn.created"], purchaseReturnInvoiceId));
    }

    public async Task<IResult<string>> UpdateAsync(CreateOrUpdatePurchaseReturnRequest request)
    {
        var purchases = _context.PurchaseReturns.Where(x => x.Id == request.Id).FirstOrDefault();
        if (purchases == null)
            throw new NotFoundException(string.Format(_localizer["purchasereturn.notfound"], request.Id));

        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var purchase = purchases.Update(referenceNo: request.ReferenceNo, storeId: request.StoreId, supplierId: request.SupplierId, purchaseReturnDate: DateTime.Parse(request.PurchaseReturnDate), purchaseReturnStatus: request.PurchaseReturnStatus, totalQuantity: request.TotalQuantity, cGST: request.CGST, sGST: request.SGST, totalTax: request.TotalTax, otherCharge: request.OtherCharge, discount: request.Discount, discountType: request.DiscountType, totalDiscount: request.TotalDiscount, subtotalAmount: request.SubtotalAmount, totalAmount: request.TotalAmount, note: request.Note);
                _context.PurchaseReturns.Update(purchase);
                int result = await _context.SaveChangesAsync();

                if (purchase.Id != Guid.Empty)
                {
                    foreach (var prod in _context.PurchaseReturnProducts.Where(c => c.PurchaseReturnId == purchase.Id))
                    {
                        if (!request.PurchaseReturnProducts.Any(p => p.Id == prod.Id))
                        {
                            var prodQty = _context.ProductQuantities.FirstOrDefault(c => c.Type == (short)ProductType.PurchaseReturn && c.TypeId == purchase.Id && c.ProductId == prod.ProductId && c.StoreId == purchase.StoreId);
                            if (prodQty != null)
                            {
                                _context.ProductQuantities.Remove(prodQty);
                            }

                            _context.PurchaseReturnProducts.Remove(prod);
                        }
                    }

                    foreach (PurchaseReturnProductRequest pp in request.PurchaseReturnProducts)
                    {
                        if (pp.Id != null && pp.Id != Guid.Empty)
                        {
                            var product = _context.PurchaseReturnProducts.FirstOrDefault(x => x.Id == pp.Id);
                            if (product == null)
                            {
                                transaction.Rollback();
                                throw new NotFoundException(string.Format(_localizer["purchasereturn.invalid"], pp.Id));
                            }

                            product.Update(productId: pp.ProductId, productName: pp.ProductName, quantity: pp.Quantity, purchasePrice: pp.PurchasePrice, discount: pp.Discount, discountType: pp.DiscountType, totalDiscount: pp.TotalDiscount, cGST: pp.CGST, sGST: pp.SGST, taxType: pp.TaxType, totalTax: pp.TotalTax, unitCostAfterTaxAndDiscount: pp.UnitCostAfterTaxAndDiscount, totalAmount: pp.TotalAmount);
                            _context.PurchaseReturnProducts.Update(product);

                            var prodQty = _context.ProductQuantities.FirstOrDefault(c => c.Type == (short)ProductType.PurchaseReturn && c.TypeId == purchase.Id && c.ProductId == pp.ProductId && c.StoreId == purchase.StoreId);
                            if (prodQty != null)
                            {
                                prodQty.Update(quantity: pp.Quantity);
                                _context.ProductQuantities.Update(prodQty);
                            }
                        }
                        else
                        {
                            var purduct = new PurchaseReturnProduct(purchaseReturnId: purchase.Id, productId: pp.ProductId, productName: pp.ProductName, quantity: pp.Quantity, purchasePrice: pp.PurchasePrice, discount: pp.Discount, discountType: pp.DiscountType, totalDiscount: pp.TotalDiscount, cGST: pp.CGST, sGST: pp.SGST, taxType: pp.TaxType, totalTax: pp.TotalTax, unitCostAfterTaxAndDiscount: pp.UnitCostAfterTaxAndDiscount, totalAmount: pp.TotalAmount);
                            await _context.PurchaseReturnProducts.AddAsync(purduct);

                            var purductQuantity = new ProductQuantiy(productId: pp.ProductId, storeId: purchase.StoreId, quantity: pp.Quantity, typeId: purchase.Id, type: (short)ProductType.PurchaseReturn);
                            await _context.ProductQuantities.AddAsync(purductQuantity);
                        }
                    }

                    if (request.PurchaseReturnPayments != null)
                    {
                        var payment = new PurchaseReturnPayment(purchaseReturnId: purchase.Id, supplierId: purchase.SupplierId, paymentTypeId: request.PurchaseReturnPayments.PaymentTypeId, amount: request.PurchaseReturnPayments.Amount, paymentDate: DateTime.Now, note: request.PurchaseReturnPayments.Note);
                        await _context.PurchaseReturnPayments.AddAsync(payment);
                    }

                    await _context.SaveChangesAsync();
                }

                transaction.Commit();
                transaction.Dispose();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction.Dispose();
                throw;
            }
        }

        return await Result<string>.SuccessAsync(string.Format(_localizer["purchasereturn.updated"], purchases.PurchaseInvoiceId));
    }

    public async Task<long> GeneratePurchaseReturnInvoiceId()
    {
        long lastcode = 0;
        var purchase = await _context.PurchaseReturns
            .AsNoTracking()
            .OrderByDescending(a => a.PRIId)
            .Take(1).SingleOrDefaultAsync();

        if (purchase != null)
        {
            lastcode = purchase.PRIId;
        }

        lastcode = lastcode + 1;

        return lastcode;
    }

    public async Task<PurchaseReturnDto> GetPurchaseReturnById(Guid id)
    {
        return null;
    }

    public async Task<IResult<string>> DeleteAsync(Guid id)
    {
        var purchases = _context.PurchaseReturns.Where(x => x.Id == id).FirstOrDefault();
        if (purchases == null)
            throw new NotFoundException(string.Format(_localizer["purchasereturn.notfound"], id));

        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        {
            try
            {
                if (purchases.Id != Guid.Empty)
                {
                    foreach (var prod in _context.PurchaseReturnProducts.Where(c => c.PurchaseReturnId == purchases.Id))
                    {
                        _context.PurchaseReturnProducts.Remove(prod);
                    }

                    foreach (var prodQty in _context.ProductQuantities.Where(c => c.Type == (short)ProductType.PurchaseReturn && c.TypeId == purchases.Id && c.StoreId == purchases.StoreId))
                    {
                        _context.ProductQuantities.Remove(prodQty);
                    }

                    foreach (var pay in _context.PurchaseReturnPayments.Where(c => c.PurchaseReturnId == purchases.Id))
                    {
                        _context.PurchaseReturnPayments.Remove(pay);
                    }

                    await _context.SaveChangesAsync();
                }

                _context.PurchaseReturns.Remove(purchases);
                int result = await _context.SaveChangesAsync();

                transaction.Commit();
                transaction.Dispose();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction.Dispose();
                throw;
            }
        }

        return await Result<string>.SuccessAsync(string.Format(_localizer["purchasereturn.deleted"], purchases.PurchaseInvoiceId));
    }

    public async Task<Result<string>> GetPurchaseReturnIdbyPurchaseInvoiceId(string id)
    {
        var purchaeReturn = _context.PurchaseReturns.FirstOrDefault(x => x.PurchaseInvoiceId == id);
        if (purchaeReturn != null)
        {
            //  return purchaeReturn.Id.ToString();
            return await Result<string>.SuccessAsync(purchaeReturn.Id.ToString(), "");
        }

        return await Result<string>.FailAsync("");
    }
}
