using HRM.API.Domain.Configuration;

namespace HRM.API.Application.Configuration;

public class GetConfigurationRequest : IRequest<List<ConfigurationDto>>
{
}

public class CountersRequestSpec : Specification<GeneralConfiguration, ConfigurationDto>
{
    public CountersRequestSpec()
    {
        Query
           .OrderBy(c => c.SortOrder);
    }
}

public class GetConfigurationRequestHandler : IRequestHandler<GetConfigurationRequest, List<ConfigurationDto>>
{
    private readonly IReadRepository<GeneralConfiguration> _repository;

    public GetConfigurationRequestHandler(IReadRepository<GeneralConfiguration> repository) => _repository = repository;
    public async Task<List<ConfigurationDto>> Handle(GetConfigurationRequest request, CancellationToken cancellationToken)
    {
        var spec = new CountersRequestSpec();
        return await _repository.ListAsync<ConfigurationDto>(spec, cancellationToken);
    }
}