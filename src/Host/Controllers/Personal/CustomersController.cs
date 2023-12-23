using HRM.API.Application.Common.Wrapper;
using HRM.API.Application.Identity.Customers;
using Microsoft.Extensions.Localization;

namespace HRM.API.Host.Controllers.Identity;

public class CustomersController : VersionedApiController
{
    private readonly IStringLocalizer<CustomersController> _localizer;
    public CustomersController(IStringLocalizer<CustomersController> localizer)
    {
        (_localizer) = (localizer);
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Customers)]
    [OpenApiOperation("Search Customers using available filters.", "")]
    public Task<PaginationResponse<CustomerDto>> SearchAsync(SearchCustomersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Customers)]
    [OpenApiOperation("Get Customer details.", "")]
    public Task<CustomerDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetCustomerRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Customers)]
    [OpenApiOperation("Create a new Customer.", "")]
    public async Task<ActionResult> CreateAsync(CreateCustomerRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["customer.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Customers)]
    [OpenApiOperation("Update a Customer.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateCustomerRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["customer.updated"], request.Name)));

    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Customers)]
    [OpenApiOperation("active/inactive a Customer.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusCustomerRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["customer.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Customers)]
    [OpenApiOperation("Delete a Customer.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeleteCustomerRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["customer.deleted"], name)));
    }

}
