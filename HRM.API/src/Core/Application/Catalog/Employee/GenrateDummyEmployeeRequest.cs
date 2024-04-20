
using HRM.API.Application.Utilities;
using Mapster;

namespace HRM.API.Application.Catalog.Employees;
public class GenrateDummyEmployeeRequest : IRequest<List<EmployeeDto>>
{
    public int? NumberOfEmployee { get; set; }
}

public class GenrateDummyEmployeeRequestHandler : IRequestHandler<GenrateDummyEmployeeRequest, List<EmployeeDto>>
{
    public async Task<List<EmployeeDto>> Handle(GenrateDummyEmployeeRequest request, CancellationToken cancellationToken)
    {
        return DataGenerator.GetEmployeeGenerator().Generate(request.NumberOfEmployee ?? 1).Adapt<List<EmployeeDto>>();
    }
}
