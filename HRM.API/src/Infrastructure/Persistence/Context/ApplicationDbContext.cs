using Finbuckle.MultiTenant;
using HRM.API.Application.Common.Events;
using HRM.API.Application.Common.Interfaces;
using HRM.API.Domain.Catalog;
using HRM.API.Domain.Configuration;
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
    public DbSet<State> States => Set<State>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Employee> Employee => Set<Employee>();
    public DbSet<Holidays> Holidays => Set<Holidays>();
    public DbSet<Project> Project => Set<Project>();
    public DbSet<Attendance> Attendance => Set<Attendance>();
    public DbSet<Priority> Priority => Set<Priority>();
    public DbSet<RateType> RateType => Set<RateType>();
    public DbSet<TimeSheet> TimeSheet => Set<TimeSheet>();
    public DbSet<Overtime> Overtime => Set<Overtime>();
    public DbSet<ProjectTaskBoard> ProjectTaskBoard => Set<ProjectTaskBoard>();
    public DbSet<GeneralConfiguration> GeneralConfigurations => Set<GeneralConfiguration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.HRM);
    }
}