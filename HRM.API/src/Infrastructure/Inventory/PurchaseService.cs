using Finbuckle.MultiTenant;
using HRM.API.Application.Common.Events;
using HRM.API.Application.Common.Exceptions;
using HRM.API.Application.Common.Interfaces;
using HRM.API.Application.Common.Wrapper;
using HRM.API.Application.Inventory;
using HRM.API.Domain.Common;
using HRM.API.Domain.Inventory;
using HRM.API.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;

namespace HRM.API.Infrastructure.Inventory;
public class PurchaseService : IPurchaseService
{
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<PurchaseService> _localizer;
    private readonly ICurrentUser _currentUser;
    private readonly ITenantInfo _currentTenant;
    private readonly IEventPublisher _events;

    public PurchaseService(
        ApplicationDbContext context,
        IStringLocalizer<PurchaseService> localizer,
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

    public async Task<IResult<Guid>> CreateAsync(CreateOrUpdatePurchaseRequest request)
    {
        Guid purchaseId = Guid.Empty;
        long pIId = await GeneratePurchaseInvoiceId();

        string purchaseInvoiceId = string.Empty;
        if (pIId >= 999)
            purchaseInvoiceId = Convert.ToString(pIId);
        else
            purchaseInvoiceId = Convert.ToString(pIId).PadLeft(4, '0');

        purchaseInvoiceId = string.Format("{0}{1}", GlobalConstant.PrefixPurchaseInvoice, purchaseInvoiceId);

        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var purchase = new Purchase(pIId, purchaseInvoiceId: purchaseInvoiceId, referenceNo: request.ReferenceNo, storeId: request.StoreId, supplierId: request.SupplierId, purchaseDate: DateTime.Parse(request.PurchaseDate), purchaseStatus: request.PurchaseStatus, totalQuantity: request.TotalQuantity, cGST: request.CGST, sGST: request.SGST, totalTax: request.TotalTax, otherCharge: request.OtherCharge, discount: request.Discount, discountType: request.DiscountType, totalDiscount: request.TotalDiscount, subtotalAmount: request.SubtotalAmount, totalAmount: request.TotalAmount, note: request.Note);
                await _context.Purchases.AddAsync(purchase);
                int result = await _context.SaveChangesAsync();
                if (result < 0)
                {
                    transaction.Rollback();
                }

                purchaseId = purchase.Id;
                if (purchase.Id != Guid.Empty)
                {
                    foreach (PurchaseProductRequest pp in request.PurchaseProducts)
                    {
                        var purduct = new PurchaseProduct(purchaseId: purchaseId, productId: pp.ProductId, productName: pp.ProductName, quantity: pp.Quantity, purchasePrice: pp.PurchasePrice, discount: pp.Discount, discountType: pp.DiscountType, totalDiscount: pp.TotalDiscount, cGST: pp.CGST, sGST: pp.SGST, taxType: pp.TaxType, totalTax: pp.TotalTax, unitCostAfterTaxAndDiscount: pp.UnitCostAfterTaxAndDiscount, totalAmount: pp.TotalAmount);
                        await _context.PurchaseProducts.AddAsync(purduct);

                        var purductQuantity = new ProductQuantiy(productId: pp.ProductId, storeId: request.StoreId, quantity: pp.Quantity, typeId: purchaseId, type: (short)ProductType.Purchase);
                        await _context.ProductQuantities.AddAsync(purductQuantity);
                    }

                    if (request.PurchasePayments != null)
                    {
                        var payment = new PurchasePayment(purchaseId: purchaseId, supplierId: purchase.SupplierId, paymentTypeId: request.PurchasePayments.PaymentTypeId, amount: request.PurchasePayments.Amount, paymentDate: DateTime.Now, note: request.PurchasePayments.Note);
                        await _context.PurchasePayments.AddAsync(payment);
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

        return await Result<Guid>.SuccessAsync(purchaseId, string.Format(_localizer["purchase.created"], purchaseInvoiceId));
    }

    public async Task<IResult<string>> UpdateAsync(CreateOrUpdatePurchaseRequest request)
    {
        var purchases = _context.Purchases.Where(x => x.Id == request.Id).FirstOrDefault();
        if (purchases == null)
            throw new NotFoundException(string.Format(_localizer["purchase.notfound"], request.Id));

        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var purchase = purchases.Update(referenceNo: request.ReferenceNo, storeId: request.StoreId, supplierId: request.SupplierId, purchaseDate: DateTime.Parse(request.PurchaseDate), purchaseStatus: request.PurchaseStatus, totalQuantity: request.TotalQuantity, cGST: request.CGST, sGST: request.SGST, totalTax: request.TotalTax, otherCharge: request.OtherCharge, discount: request.Discount, discountType: request.DiscountType, totalDiscount: request.TotalDiscount, subtotalAmount: request.SubtotalAmount, totalAmount: request.TotalAmount, note: request.Note);
                _context.Purchases.Update(purchase);
                int result = await _context.SaveChangesAsync();

                if (purchase.Id != Guid.Empty)
                {
                    foreach (var prod in _context.PurchaseProducts.Where(c => c.PurchaseId == purchase.Id))
                    {
                        if (!request.PurchaseProducts.Any(p => p.Id == prod.Id))
                        {
                            var prodQty = _context.ProductQuantities.FirstOrDefault(c => c.Type == (short)ProductType.Purchase && c.TypeId == purchase.Id && c.ProductId == prod.ProductId && c.StoreId == purchase.StoreId);
                            if (prodQty != null)
                            {
                                _context.ProductQuantities.Remove(prodQty);
                            }

                            _context.PurchaseProducts.Remove(prod);
                        }
                    }

                    foreach (PurchaseProductRequest pp in request.PurchaseProducts)
                    {
                        if (pp.Id != null && pp.Id != Guid.Empty)
                        {
                            var product = _context.PurchaseProducts.FirstOrDefault(x => x.Id == pp.Id);
                            if (product == null)
                            {
                                transaction.Rollback();
                                throw new NotFoundException(string.Format(_localizer["purchase.invalid"], pp.Id));
                            }

                            product.Update(productId: pp.ProductId, productName: pp.ProductName, quantity: pp.Quantity, purchasePrice: pp.PurchasePrice, discount: pp.Discount, discountType: pp.DiscountType, totalDiscount: pp.TotalDiscount, cGST: pp.CGST, sGST: pp.SGST, taxType: pp.TaxType, totalTax: pp.TotalTax, unitCostAfterTaxAndDiscount: pp.UnitCostAfterTaxAndDiscount, totalAmount: pp.TotalAmount);
                            _context.PurchaseProducts.Update(product);

                            var prodQty = _context.ProductQuantities.FirstOrDefault(c => c.Type == (short)ProductType.Purchase && c.TypeId == purchase.Id && c.ProductId == pp.ProductId && c.StoreId == purchase.StoreId);
                            if (prodQty != null)
                            {
                                prodQty.Update(quantity: pp.Quantity);
                                _context.ProductQuantities.Update(prodQty);
                            }
                        }
                        else
                        {
                            var purduct = new PurchaseProduct(purchaseId: purchase.Id, productId: pp.ProductId, productName: pp.ProductName, quantity: pp.Quantity, purchasePrice: pp.PurchasePrice, discount: pp.Discount, discountType: pp.DiscountType, totalDiscount: pp.TotalDiscount, cGST: pp.CGST, sGST: pp.SGST, taxType: pp.TaxType, totalTax: pp.TotalTax, unitCostAfterTaxAndDiscount: pp.UnitCostAfterTaxAndDiscount, totalAmount: pp.TotalAmount);
                            await _context.PurchaseProducts.AddAsync(purduct);

                            var purductQuantity = new ProductQuantiy(productId: pp.ProductId, storeId: purchase.StoreId, quantity: pp.Quantity, typeId: purchase.Id, type: (short)ProductType.Purchase);
                            await _context.ProductQuantities.AddAsync(purductQuantity);
                        }
                    }

                    if (request.PurchasePayments != null)
                    {
                        var payment = new PurchasePayment(purchaseId: purchase.Id, supplierId: purchase.SupplierId, paymentTypeId: request.PurchasePayments.PaymentTypeId, amount: request.PurchasePayments.Amount, paymentDate: DateTime.Now, note: request.PurchasePayments.Note);
                        await _context.PurchasePayments.AddAsync(payment);
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

        return await Result<string>.SuccessAsync(string.Format(_localizer["purchase.updated"], purchases.PurchaseInvoiceId));
    }

    public async Task<long> GeneratePurchaseInvoiceId()
    {
        long lastcode = 0;
        var purchase = await _context.Purchases
            .AsNoTracking()
            .OrderByDescending(a => a.PIId)
            .Take(1).SingleOrDefaultAsync();

        if (purchase != null)
        {
            lastcode = purchase.PIId;
        }

        lastcode = lastcode + 1;

        return lastcode;
    }

    public async Task<PurchaseDto> GetPurchaseById(Guid id)
    {
        return null;
    }

    public async Task<IResult<string>> DeleteAsync(Guid id)
    {
        var purchases = _context.Purchases.Where(x => x.Id == id).FirstOrDefault();
        if (purchases == null)
            throw new NotFoundException(string.Format(_localizer["purchase.notfound"], id));

        using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
        {
            try
            {
                if (purchases.Id != Guid.Empty)
                {
                    foreach (var prod in _context.PurchaseProducts.Where(c => c.PurchaseId == purchases.Id))
                    {
                        _context.PurchaseProducts.Remove(prod);
                    }

                    foreach (var prodQty in _context.ProductQuantities.Where(c => c.Type == (short)ProductType.Purchase && c.TypeId == purchases.Id && c.StoreId == purchases.StoreId))
                    {
                        _context.ProductQuantities.Remove(prodQty);
                    }

                    foreach (var pay in _context.PurchasePayments.Where(c => c.PurchaseId == purchases.Id))
                    {
                        _context.PurchasePayments.Remove(pay);
                    }

                    await _context.SaveChangesAsync();
                }

                _context.Purchases.Remove(purchases);
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

        return await Result<string>.SuccessAsync(string.Format(_localizer["purchase.deleted"], purchases.PurchaseInvoiceId));
    }

}