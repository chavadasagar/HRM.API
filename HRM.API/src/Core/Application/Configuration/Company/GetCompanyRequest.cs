using HRM.API.Domain.Configuration;

namespace HRM.API.Application.Configuration;

public class GetCompanyRequest : IRequest<CompanyDto>
{
}

public class CompanySpec : Specification<Company, CompanyDto>, ISingleResultSpecification
{
    public CompanySpec()
    {

    }
}

public class GetCompanyRequestHandler : IRequestHandler<GetCompanyRequest, CompanyDto>
{
    private readonly IRepository<Company> _repository;
    private readonly IStringLocalizer<GetCompanyRequestHandler> _localizer;

    public GetCompanyRequestHandler(IRepository<Company> repository, IStringLocalizer<GetCompanyRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<CompanyDto> Handle(GetCompanyRequest request, CancellationToken cancellationToken)
    {
        return await _repository.GetBySpecAsync(
            (ISpecification<Company, CompanyDto>)new CompanySpec(), cancellationToken)
             ?? throw new NotFoundException(string.Format(_localizer["brand.notfound"]));
    }
}