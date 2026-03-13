namespace OMB.Api.DTOs;

public class CreateLocationDto
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public bool Active { get; set; } = true;
}