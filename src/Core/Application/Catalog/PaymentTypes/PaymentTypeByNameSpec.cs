namespace HRM.API.Application.Catalog.PaymentTypes;

public class PaymentTypeByNameSpec : Specification<PaymentType>, ISingleResultSpecification
{
    public PaymentTypeByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}