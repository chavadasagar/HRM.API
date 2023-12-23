using Finbuckle.MultiTenant.EntityFrameworkCore;
using HRM.API.Domain.Catalog;
using HRM.API.Domain.Configuration;
using HRM.API.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRM.API.Infrastructure.Persistence.Configuration;

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
    }
}

public class StateConfig : IEntityTypeConfiguration<States>
{
    public void Configure(EntityTypeBuilder<States> builder)
    {
        builder
           .Property(b => b.Name)
               .HasMaxLength(256);

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