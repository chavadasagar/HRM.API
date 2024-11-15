using MasterPOS.API.Application.Multitenancy;
using MasterPOS.API.Domain.Configuration;

namespace MasterPOS.API.Application.Configuration;
public class UpdateCompanyRequest : IRequest<string>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? DirectorName { get; set; }
    public string? Mobile { get; set; }
    public string Email { get; set; } = default!;
    public string? Phone { get; set; }
    public string? GSTNumber { get; set; }
    public string? VATNumber { get; set; }
    public string? PANNumber { get; set; }
    public string? BankDetails { get; set; }
    public string? Website { get; set; }
    public string? UPIId { get; set; }
    public Guid? CountryId { get; set; }
    public Guid? StateId { get; set; }
    public string? City { get; set; }
    public string? Postcode { get; set; }
    public string? Address { get; set; }
    public FileUploadRequest? CompanyLogoImage { get; set; }
}

public class UpdateCompanyRequestHandler : IRequestHandler<UpdateCompanyRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Company> _repository;
    private readonly IStringLocalizer<UpdateCompanyRequestHandler> _localizer;
    private readonly IFileStorageService _file;
    private readonly ITenantService _tenantService;

    public UpdateCompanyRequestHandler(IRepositoryWithEvents<Company> repository, IStringLocalizer<UpdateCompanyRequestHandler> localizer, IFileStorageService file, ITenantService tenantService) =>
        (_repository, _localizer, _file, _tenantService) = (repository, localizer, file, tenantService);

    public async Task<string> Handle(UpdateCompanyRequest request, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = company ?? throw new NotFoundException(string.Format(_localizer["company.notfound"], request.Id));

        // Remove old image if flag is set
        string? currentcompanyLogoPath = company.CompanyLogoPath;
        if (request.CompanyLogoImage != null && request.CompanyLogoImage.Data != null)
        {
            if (!string.IsNullOrEmpty(currentcompanyLogoPath))
            {
                string root = Directory.GetCurrentDirectory();
                _file.Remove(Path.Combine(root, currentcompanyLogoPath));
            }

            company = company.ClearImagePath();
        }

        string? companyImagePath = request.CompanyLogoImage is not null
            ? await _file.UploadAsync<Company>(request.CompanyLogoImage, GlobalConstant.CompanyImageUploadDirectory, FileType.Image, cancellationToken)
            : null;

        company = company.Update(name: request.Name, directorName: request.DirectorName, email: request.Email, mobile: request.Mobile, phone: request.Phone, gSTNumber: request.GSTNumber, vATNumber: request.VATNumber, pANNumber: request.PANNumber, website: request.Website, uPIId: request.UPIId, bankDetails: request.BankDetails, countryId: request.CountryId, stateId: request.StateId, city: request.City, postcode: request.Postcode, address: request.Address, companyLogoPath: companyImagePath ?? currentcompanyLogoPath);
        await _repository.UpdateAsync(company, cancellationToken);

        return "updated";
    }
}
