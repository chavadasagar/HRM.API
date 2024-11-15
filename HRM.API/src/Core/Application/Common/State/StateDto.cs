namespace MasterPOS.API.Application.Common;
public class StateDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
