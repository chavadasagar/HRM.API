using Finbuckle.MultiTenant;
using HRM.API.Application.Common.Events;
using HRM.API.Application.Common.Interfaces;
using HRM.API.Domain.Catalog;
using HRM.API.Domain.Configuration;
using HRM.API.Domain.Identity;
using HRM.API.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HRM.API.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<States> States => Set<States>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<GeneralConfiguration> GeneralConfigurations => Set<GeneralConfiguration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.HRM);
    }
}