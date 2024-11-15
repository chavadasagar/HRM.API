using MasterPOS.API.Application.Catalog.PaymentTypes;
using MasterPOS.API.Application.Common.Wrapper;
using Microsoft.Extensions.Localization;

namespace MasterPOS.API.Host.Controllers.Catalog;

public class PaymentTypesController : VersionedApiController
{
    private readonly IStringLocalizer<PaymentTypesController> _localizer;
    public PaymentTypesController(IStringLocalizer<PaymentTypesController> localizer)
    {
        _localizer = localizer;
    }

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.PaymentTypes)]
    [OpenApiOperation("Search payment types using available filters.", "")]
    public Task<PaginationResponse<PaymentTypeDto>> SearchAsync(SearchPaymentTypesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.PaymentTypes)]
    [OpenApiOperation("Get payment type details.", "")]
    public Task<PaymentTypeDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetPaymentTypeRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.PaymentTypes)]
    [OpenApiOperation("Create a new payment type.", "")]
    public async Task<ActionResult> CreateAsync(CreatePaymentTypeRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["paymenttype.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.PaymentTypes)]
    [OpenApiOperation("Update a payment type.", "")]
    public async Task<ActionResult> UpdateAsync(UpdatePaymentTypeRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();
        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["paymenttype.updated"], request.Name)));

    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.PaymentTypes)]
    [OpenApiOperation("active/inactive a payment type.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusPaymentTypeRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["paymenttype.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.PaymentTypes)]
    [OpenApiOperation("Delete a payment type.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeletePaymentTypeRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["paymenttype.deleted"], name)));
    }

}