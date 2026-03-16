using System.ComponentModel;

namespace OMB.Api.DTOs
{
    public class CreateResidentDto
    {

        public long LocationId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int? IddsiLevel { get; set; }
        [DefaultValue(false)] // standaardwaarde voor vegetarisch is false, dit moet worden meegegeven anders springt hij weer op true. :')
        public bool IsVegetarian { get; set; } = false;
        public string? AllergenNotes { get; set; }
        public bool IsActive { get; set; } = true;
    }
}