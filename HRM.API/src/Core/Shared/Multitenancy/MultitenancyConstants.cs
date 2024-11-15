namespace MasterPOS.API.Shared.Multitenancy;

public class MultitenancyConstants
{
    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "admin@root.com";
    }

    public const string DefaultPassword = "123Pa$$word!";

    public const string TenantIdName = "tenant";

    public const string DefaultCustomer = "Walk-in-customer";
    public const string DefaultStoreCode = "ST0001";

}