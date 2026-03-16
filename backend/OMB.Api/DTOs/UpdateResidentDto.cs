using System.ComponentModel;

namespace OMB.Api.DTOs;

public class UpdateResidentDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public long LocationId { get; set; }
    public int? IddsiLevel { get; set; }
    [DefaultValue(false)] // standaardwaarde voor vegetarisch is false
    public bool IsVegetarian { get; set; }
    public string? AllergenNotes { get; set; }
    public bool IsActive { get; set; }
}