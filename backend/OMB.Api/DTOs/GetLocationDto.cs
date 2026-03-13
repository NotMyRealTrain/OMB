namespace OMB.Api.DTOs;

public class GetLocationDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public bool Active { get; set; }
}