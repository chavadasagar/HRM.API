namespace MasterPOS.API.Domain.Inventory;
public class ProductQuantiy : AuditableEntity, IAggregateRoot
{
    public Guid ProductId { get; private set; } = default!;
    public Guid StoreId { get; private set; } = default!;
    public decimal Quantity { get; private set; } = default!;
    public Guid TypeId { get; private set; } = default!;
    public short Type { get; private set; } = default!;

    public ProductQuantiy(Guid productId, Guid storeId, decimal quantity, Guid typeId, short type)
    {
        ProductId = productId;
        StoreId = storeId;
        Quantity = quantity;
        TypeId = typeId;
        Type = type;
    }

    public ProductQuantiy Update(decimal quantity)
    {
        Quantity = quantity;

        return this;
    }
}
