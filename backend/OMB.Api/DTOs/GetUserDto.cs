namespace OMB.Api.DTOs;

public class GetUserDto
{
    public long Id { get; set; }
    public string Email { get; set; } = null!;
    public string DisplayNameFirst {get; set;} = null!;
    public string DisplayNameLast {get; set;} = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public long? DefaultLocationId { get; set; }
    public string? DefaultLocationName { get; set; }

    public List<string> Roles { get; set; } = new ();
    public List<LocationDto> Locations { get; set; } = new ();
}

public class LocationDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}