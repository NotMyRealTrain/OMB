namespace OMB.Api.DTOs.Residents;

public class GetResidentDto
{
    public long Id { get; set; }
    public long LocationId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int? IddsiLevel { get; set; }
    public bool IsVegetarian { get; set; }
    public string? AllergenNotes { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
}