using Finbuckle.MultiTenant.EntityFrameworkCore;
using MasterPOS.API.Domain.Catalog;
using MasterPOS.API.Domain.Configuration;
using MasterPOS.API.Domain.Identity;
using MasterPOS.API.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterPOS.API.Infrastructure.Persistence.Configuration;

public class BrandConfig : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(256);

        builder
          .Property(p => p.ImagePath)
              .HasMaxLength(2048);
    }
}

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(256);

        builder
           .Property(p => p.ImagePath)
               .HasMaxLength(2048);
    }
}

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(1024);

        builder
            .Property(b => b.SKU)
                .HasMaxLength(256);

        builder
            .Property(b => b.HSN)
                .HasMaxLength(256);

        builder
            .Property(b => b.Barcode)
                .HasMaxLength(256);

        builder
            .Property(p => p.ImagePath)
                .HasMaxLength(2048);
    }
}

public class UnitConfig : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(256);
    }
}

public class PaymentTypeConfig : IEntityTypeConfiguration<PaymentType>
{
    public void Configure(EntityTypeBuilder<PaymentType> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
                .HasMaxLength(256);
    }
}

public class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder
           .Property(b => b.Name)
               .HasMaxLength(256);

        builder
           .Property(b => b.NormalizedName)
               .HasMaxLength(256);
    }
}

public class StateConfig : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder
           .Property(b => b.Name)
               .HasMaxLength(256);

        builder
           .Property(b => b.NormalizedName)
               .HasMaxLength(256);
    }
}

public class StoreConfig : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.Code)
               .HasMaxLength(16);

        builder
           .Property(b => b.Name)
               .HasMaxLength(256);

        builder
           .Property(b => b.Mobile)
               .HasMaxLength(16);

        builder
           .Property(b => b.Email)
               .HasMaxLength(256);

        builder
           .Property(b => b.Phone)
               .HasMaxLength(16);

        builder
           .Property(b => b.GSTNumber)
               .HasMaxLength(32);

        builder
           .Property(b => b.PANNumber)
               .HasMaxLength(32);

        builder
           .Property(b => b.City)
               .HasMaxLength(32);

        builder
           .Property(b => b.Postcode)
               .HasMaxLength(8);
    }
}

public class CounterConfig : IEntityTypeConfiguration<Counter>
{
    public void Configure(EntityTypeBuilder<Counter> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.Name)
               .HasMaxLength(256);
    }
}

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.Name)
               .HasMaxLength(1024);

        builder
           .Property(b => b.Mobile)
               .HasMaxLength(16);

        builder
           .Property(b => b.Email)
               .HasMaxLength(256);

        builder
           .Property(b => b.Phone)
               .HasMaxLength(16);

        builder
           .Property(b => b.GSTNumber)
               .HasMaxLength(32);

        builder
           .Property(b => b.City)
               .HasMaxLength(32);

        builder
           .Property(b => b.Postcode)
               .HasMaxLength(8);
    }
}

public class ConfigurationConfig : IEntityTypeConfiguration<GeneralConfiguration>
{
    public void Configure(EntityTypeBuilder<GeneralConfiguration> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.ConfigKey)
               .HasMaxLength(512);

        builder
           .Property(b => b.ConfigText)
               .HasMaxLength(512);

        builder
           .Property(b => b.ConfigValue)
               .HasMaxLength(512);
    }
}

public class CompanyConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.Name)
               .HasMaxLength(256);

        builder
           .Property(b => b.DirectorName)
               .HasMaxLength(100);

        builder
           .Property(b => b.Mobile)
               .HasMaxLength(16);

        builder
           .Property(b => b.Email)
               .HasMaxLength(256);

        builder
           .Property(b => b.Phone)
               .HasMaxLength(16);

        builder
           .Property(b => b.GSTNumber)
               .HasMaxLength(32);

        builder
           .Property(b => b.VATNumber)
               .HasMaxLength(32);

        builder
           .Property(b => b.PANNumber)
               .HasMaxLength(32);

        builder
           .Property(b => b.UPIId)
               .HasMaxLength(256);

        builder
           .Property(b => b.City)
               .HasMaxLength(32);

        builder
           .Property(b => b.Postcode)
               .HasMaxLength(8);
    }
}

public class SupplierConfig : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.Name)
               .HasMaxLength(1024);

        builder
           .Property(b => b.Mobile)
               .HasMaxLength(16);

        builder
           .Property(b => b.Email)
               .HasMaxLength(256);

        builder
           .Property(b => b.Phone)
               .HasMaxLength(16);

        builder
           .Property(b => b.GSTNumber)
               .HasMaxLength(32);

        builder
           .Property(b => b.City)
               .HasMaxLength(32);

        builder
           .Property(b => b.Postcode)
               .HasMaxLength(8);
    }
}

public class PurchaseConfig : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.PurchaseInvoiceId)
               .HasMaxLength(128);

        builder
           .Property(b => b.ReferenceNo)
               .HasMaxLength(512);
    }
}

public class PurchaseProductConfig : IEntityTypeConfiguration<PurchaseProduct>
{
    public void Configure(EntityTypeBuilder<PurchaseProduct> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.ProductName)
               .HasMaxLength(1024);
    }
}

public class PurchasePaymentConfig : IEntityTypeConfiguration<PurchasePayment>
{
    public void Configure(EntityTypeBuilder<PurchasePayment> builder)
    {
        builder.IsMultiTenant();
        builder.Property(x => x.Amount).HasPrecision(18, 4);
    }
}

public class PurchaseReturnConfig : IEntityTypeConfiguration<PurchaseReturn>
{
    public void Configure(EntityTypeBuilder<PurchaseReturn> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.PurchaseReturnInvoiceId)
               .HasMaxLength(128);

        builder
           .Property(b => b.PurchaseInvoiceId)
               .HasMaxLength(128);

        builder
           .Property(b => b.ReferenceNo)
               .HasMaxLength(512);
    }
}

public class PurchaseReturnProductConfig : IEntityTypeConfiguration<PurchaseReturnProduct>
{
    public void Configure(EntityTypeBuilder<PurchaseReturnProduct> builder)
    {
        builder.IsMultiTenant();

        builder
           .Property(b => b.ProductName)
               .HasMaxLength(1024);
    }
}

public class PurchaseReturnPaymentConfig : IEntityTypeConfiguration<PurchaseReturnPayment>
{
    public void Configure(EntityTypeBuilder<PurchaseReturnPayment> builder)
    {
        builder.IsMultiTenant();
        builder.Property(x => x.Amount).HasPrecision(18, 4);
    }
}

public class ProductQuantiyConfig : IEntityTypeConfiguration<ProductQuantiy>
{
    public void Configure(EntityTypeBuilder<ProductQuantiy> builder)
    {
        builder.IsMultiTenant();
    }
}