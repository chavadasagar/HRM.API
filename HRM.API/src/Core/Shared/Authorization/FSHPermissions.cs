using System.Collections.ObjectModel;

namespace MasterPOS.API.Shared.Authorization;

public static class FSHAction
{
    public const string View = nameof(View);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string UpdateStatus = nameof(UpdateStatus);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class FSHResource
{
    public const string Tenants = nameof(Tenants);
    public const string Company = nameof(Company);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Purchases = nameof(Purchases);
    public const string PurchaseReturn = nameof(PurchaseReturn);
    public const string Brands = nameof(Brands);
    public const string Category = nameof(Category);
    public const string Units = nameof(Units);
    public const string PaymentTypes = nameof(PaymentTypes);
    public const string Stores = nameof(Stores);
    public const string Counters = nameof(Counters);
    public const string Customers = nameof(Customers);
    public const string Configuration = nameof(Configuration);
    public const string Suppliers = nameof(Suppliers);
}

public static class FSHPermissions
{
    private static readonly FSHPermission[] _all = new FSHPermission[]
    {
        // Dashboard
        new("View Dashboard", FSHAction.View, FSHResource.Dashboard),

        // Hangfire
        new("View Hangfire", FSHAction.View, FSHResource.Hangfire),

        // Users
        new("View Users", FSHAction.View, FSHResource.Users),
        new("Create Users", FSHAction.Create, FSHResource.Users),
        new("Update Users", FSHAction.Update, FSHResource.Users),
        new("UpdateStatus Users", FSHAction.UpdateStatus, FSHResource.Users),
        new("Delete Users", FSHAction.Delete, FSHResource.Users),
        //new("Export Users", FSHAction.Export, FSHResource.Users),

        // UserRoles
        new("View UserRoles", FSHAction.View, FSHResource.UserRoles),
        new("Update UserRoles", FSHAction.Update, FSHResource.UserRoles),

        // Roles
        new("View Roles", FSHAction.View, FSHResource.Roles),
        new("Create Roles", FSHAction.Create, FSHResource.Roles),
        new("Update Roles", FSHAction.Update, FSHResource.Roles),
       // new("UpdateStatus Roles", FSHAction.UpdateStatus, FSHResource.Roles),
        new("Delete Roles", FSHAction.Delete, FSHResource.Roles),

        // RoleClaims
        new("View RoleClaims", FSHAction.View, FSHResource.RoleClaims),
        new("Update RoleClaims", FSHAction.Update, FSHResource.RoleClaims),

        // Products
        new("View Products", FSHAction.View, FSHResource.Products, IsBasic: true),
        new("Create Products", FSHAction.Create, FSHResource.Products),
        new("Update Products", FSHAction.Update, FSHResource.Products),
        new("UpdateStatus Products", FSHAction.UpdateStatus, FSHResource.Products),
        new("Delete Products", FSHAction.Delete, FSHResource.Products),
        //new("Export Products", FSHAction.Export, FSHResource.Products),

        // Brands
        new("View Brands", FSHAction.View, FSHResource.Brands, IsBasic: true),
        new("Create Brands", FSHAction.Create, FSHResource.Brands),
        new("Update Brands", FSHAction.Update, FSHResource.Brands),
        new("UpdateStatus Brands", FSHAction.UpdateStatus, FSHResource.Brands),
        new("Delete Brands", FSHAction.Delete, FSHResource.Brands),
        //new("Generate Brands", FSHAction.Generate, FSHResource.Brands),
        //new("Clean Brands", FSHAction.Clean, FSHResource.Brands),

        // Category
        new("View Category", FSHAction.View, FSHResource.Category, IsBasic: true),
        new("Create Category", FSHAction.Create, FSHResource.Category),
        new("Update Category", FSHAction.Update, FSHResource.Category),
        new("UpdateStatus Category", FSHAction.UpdateStatus, FSHResource.Category),
        new("Delete Category", FSHAction.Delete, FSHResource.Category),

        // Units
        new("View Units", FSHAction.View, FSHResource.Units, IsBasic: true),
        new("Create Units", FSHAction.Create, FSHResource.Units),
        new("Update Units", FSHAction.Update, FSHResource.Units),
        new("UpdateStatus Units", FSHAction.UpdateStatus, FSHResource.Units),
        new("Delete Units", FSHAction.Delete, FSHResource.Units),

        // PaymentTypes
        new("View Payment Types", FSHAction.View, FSHResource.PaymentTypes, ResourceDescription: "Payment Types", IsBasic: true),
        new("Create Payment Types", FSHAction.Create, FSHResource.PaymentTypes, ResourceDescription: "Payment Types"),
        new("Update Payment Types", FSHAction.Update, FSHResource.PaymentTypes, ResourceDescription: "Payment Types"),
        new("UpdateStatus Payment Types", FSHAction.UpdateStatus, FSHResource.PaymentTypes, ResourceDescription: "Payment Types"),
        new("Delete Payment Types", FSHAction.Delete, FSHResource.PaymentTypes, ResourceDescription: "Payment Types"),

        // Stores
        new("View Stores", FSHAction.View, FSHResource.Stores, IsBasic: true),
        new("Create Stores", FSHAction.Create, FSHResource.Stores),
        new("Update Stores", FSHAction.Update, FSHResource.Stores),
        new("UpdateStatus Stores", FSHAction.UpdateStatus, FSHResource.Stores),
        new("Delete Stores", FSHAction.Delete, FSHResource.Stores),

        // Counters
        new("View Counters", FSHAction.View, FSHResource.Counters, IsBasic: true),
        new("Create Counters", FSHAction.Create, FSHResource.Counters),
        new("Update Counters", FSHAction.Update, FSHResource.Counters),
        new("UpdateStatus Counters", FSHAction.UpdateStatus, FSHResource.Counters),
        new("Delete Counters", FSHAction.Delete, FSHResource.Counters),

        // Customers
        new("View Customers", FSHAction.View, FSHResource.Customers, IsBasic: true),
        new("Create Customers", FSHAction.Create, FSHResource.Customers),
        new("Update Customers", FSHAction.Update, FSHResource.Customers),
        new("UpdateStatus Customers", FSHAction.UpdateStatus, FSHResource.Customers),
        new("Delete Customers", FSHAction.Delete, FSHResource.Customers),

        // Configuration
        new("View Configuration", FSHAction.View, FSHResource.Configuration, ResourceDescription: "General"),
        new("Update Configuration", FSHAction.Update, FSHResource.Configuration, ResourceDescription: "General"),

        // Company
        new("View Company", FSHAction.View, FSHResource.Company, ResourceDescription: "Company Profile"),
        new("Update Company", FSHAction.Update, FSHResource.Company, ResourceDescription: "Company Profile"),

        // Supplier
        new("View Suppliers", FSHAction.View, FSHResource.Suppliers, IsBasic: true),
        new("Create Suppliers", FSHAction.Create, FSHResource.Suppliers),
        new("Update Suppliers", FSHAction.Update, FSHResource.Suppliers),
        new("UpdateStatus Suppliers", FSHAction.UpdateStatus, FSHResource.Suppliers),
        new("Delete Suppliers", FSHAction.Delete, FSHResource.Suppliers),

        // Tenants
        new("View Tenants", FSHAction.View, FSHResource.Tenants, IsRoot: true),
        new("Create Tenants", FSHAction.Create, FSHResource.Tenants, IsRoot: true),
        new("Update Tenants", FSHAction.Update, FSHResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FSHAction.UpgradeSubscription, FSHResource.Tenants, IsRoot: true),

        // Purchases
        new("View Purchases", FSHAction.View, FSHResource.Purchases, IsBasic: true),
        new("Create Purchases", FSHAction.Create, FSHResource.Purchases),
        new("Update Purchases", FSHAction.Update, FSHResource.Purchases),
        new("Delete Purchases", FSHAction.Delete, FSHResource.Purchases),
        //new("Export Purchases", FSHAction.Export, FSHResource.Purchases),

        // Purchase Return
        new("View Purchase Return", FSHAction.View, FSHResource.PurchaseReturn, IsBasic: true),
        new("Create Purchase Return", FSHAction.Create, FSHResource.PurchaseReturn),
        new("Update Purchase Return", FSHAction.Update, FSHResource.PurchaseReturn),
        new("Delete Purchase Return", FSHAction.Delete, FSHResource.PurchaseReturn),
        //new("Export Purchase Return", FSHAction.Export, FSHResource.PurchaseReturn),
    };

    public static IReadOnlyList<FSHPermission> All { get; } = new ReadOnlyCollection<FSHPermission>(_all);
    public static IReadOnlyList<FSHPermission> Root { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FSHPermission> Admin { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FSHPermission> Basic { get; } = new ReadOnlyCollection<FSHPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record FSHPermission(string Description, string Action, string Resource, string ResourceDescription = "", bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public string DisplayText => ResourceDescription == string.Empty ? Resource : ResourceDescription;
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}