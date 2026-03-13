namespace OMB.Api.DTOs;

public class UpdateLocationDto
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public bool Active { get; set; } = true;
}