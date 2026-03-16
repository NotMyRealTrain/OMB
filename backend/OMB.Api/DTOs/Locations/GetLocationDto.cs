namespace OMB.Api.DTOs.Locations;

public class GetLocationDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public bool Active { get; set; }
}