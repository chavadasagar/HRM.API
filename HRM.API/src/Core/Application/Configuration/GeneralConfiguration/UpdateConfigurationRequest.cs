using HRM.API.Domain.Configuration;

namespace HRM.API.Application.Configuration;
public class UpdateConfigurationRequest : IRequest<string>
{
    public Guid Id { get; set; }
    public string ConfigValue { get; set; } = default!;
}

public class UpdateConfigurationRequestHandler : IRequestHandler<UpdateConfigurationRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<GeneralConfiguration> _repository;
    private readonly IStringLocalizer<UpdateConfigurationRequestHandler> _localizer;

    public UpdateConfigurationRequestHandler(IRepositoryWithEvents<GeneralConfiguration> repository, IStringLocalizer<UpdateConfigurationRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<string> Handle(UpdateConfigurationRequest request, CancellationToken cancellationToken)
    {
        var configuration = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = configuration ?? throw new NotFoundException(string.Format(_localizer["configuration.notfound"], request.Id));

        configuration.ConfigValue = request.ConfigValue;

        await _repository.UpdateAsync(configuration, cancellationToken);

        return "updated";
    }
}
