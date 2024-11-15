using HRM.API.Application.Catalog;
using HRM.API.Application.Common.Wrapper;
using HRM.API.Domain.Catalog;
using HRM.API.Host.Controllers;
using Microsoft.Extensions.Localization;

namespace FSH.WebApi.Host.Controllers.Catalog;
public class CategoryController : VersionedApiController
{
    private readonly IStringLocalizer<CategoryController> _localizer;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CategoryController(IStringLocalizer<CategoryController> localizer, IHttpContextAccessor httpContextAccessor) =>
        (_localizer, _httpContextAccessor) = (localizer, httpContextAccessor);

    [HttpPost("search")]
    [MustHavePermission(FSHAction.View, FSHResource.Category)]
    [OpenApiOperation("Search categories using available filters.", "")]
    public Task<PaginationResponse<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Category)]
    [OpenApiOperation("Get category details.", "")]
    public async Task<CategoryDto> GetAsync(Guid id)
    {
        var result = await Mediator.Send(new GetCategoryRequest(id));
        //if (result != null)
        //{
        //    string host = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/";
        //    result.ImagePath = string.IsNullOrEmpty(result.ImagePath) ? string.Empty : (host + result.ImagePath);
        //}
        return result;
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Category)]
    [OpenApiOperation("Create a new Category.", "")]
    public async Task<ActionResult> CreateAsync(CreateCategoryRequest request)
    {
        Guid id = await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["category.created"], request.Name)));
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Category)]
    [OpenApiOperation("Update a Category.", "")]
    public async Task<ActionResult> UpdateAsync(UpdateCategoryRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["category.updated"], request.Name)));
    }

    [HttpPut("updatestatus/{id:guid}")]
    [MustHavePermission(FSHAction.UpdateStatus, FSHResource.Category)]
    [OpenApiOperation("active/inactive a category.", "")]
    public async Task<ActionResult> UpdateStatusAsync(UpdateStatusCategoryRequest request, Guid id)
    {
        if (id != request.Id)
            return BadRequest();

        await Mediator.Send(request);
        return Ok(await Result<Guid>.SuccessAsync(id, _localizer["category.updatedstatus"]));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Category)]
    [OpenApiOperation("Delete a Category.", "")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        string name = await Mediator.Send(new DeleteCategoryRequest(id));
        return Ok(await Result<Guid>.SuccessAsync(id, string.Format(_localizer["category.deleted"], name)));

    }
}
