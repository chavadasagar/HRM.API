using HRM.API.Application.Common;

namespace HRM.API.Application.Common;
public class GetCityRequest : IRequest<List<CityDto>>
{
    public GetCityRequest(DefaultIdType? stateId)
    {
        StateId = stateId;
    }

    public Guid? StateId { get; set; }
}

public class GetCityValidator : CustomValidator<GetCityRequest>
{
    public GetCityValidator()
    {
        RuleFor(x => x.StateId).NotNull().NotEmpty().WithMessage("Please select state");
    }
}

public class CityRequestSpec : Specification<City, CityDto>
{
    public CityRequestSpec(Guid? stateId) =>
        Query.Where(p => p.StateId == stateId);

}

public class GetCityHandler : IRequestHandler<GetCityRequest, List<CityDto>>
{
    private readonly IRepository<City> _city;
    private readonly IStringLocalizer<GetCityHandler> _localizer;

    public GetCityHandler(IRepository<City> city, IStringLocalizer<GetCityHandler> localizer)
    {
        _city = city;
        _localizer = localizer;
    }

    public async Task<List<CityDto>> Handle(GetCityRequest request, CancellationToken cancellationToken)
    {
        var spec = new CityRequestSpec(request.StateId);

        var citys = await _city.ListAsync(spec, cancellationToken);

        return citys;
    }
}