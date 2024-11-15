using Finbuckle.MultiTenant;
using MasterPOS.API.Application.Common.Events;
using MasterPOS.API.Application.Common.Interfaces;
using MasterPOS.API.Domain.Catalog;
using MasterPOS.API.Domain.Configuration;
using MasterPOS.API.Domain.Identity;
using MasterPOS.API.Domain.Inventory;
using MasterPOS.API.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MasterPOS.API.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Counter> Counters => Set<Counter>();
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<State> States => Set<State>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<GeneralConfiguration> GeneralConfigurations => Set<GeneralConfiguration>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseProduct> PurchaseProducts => Set<PurchaseProduct>();
    public DbSet<PurchasePayment> PurchasePayments => Set<PurchasePayment>();
    public DbSet<PurchaseReturn> PurchaseReturns => Set<PurchaseReturn>();
    public DbSet<PurchaseReturnProduct> PurchaseReturnProducts => Set<PurchaseReturnProduct>();
    public DbSet<PurchaseReturnPayment> PurchaseReturnPayments => Set<PurchaseReturnPayment>();
    public DbSet<ProductQuantiy> ProductQuantities => Set<ProductQuantiy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.MPOS);
    }
}