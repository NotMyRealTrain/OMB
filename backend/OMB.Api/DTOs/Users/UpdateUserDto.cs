using System.ComponentModel.DataAnnotations;

namespace OMB.Api.DTOs.Users;

public class UpdateUserDto
{
    [Required]
    public string DisplayNameFirst { get; set; } = null!;

    [Required]
    public string DisplayNameLast { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public long? DefaultLocationId { get; set; }

    public bool IsActive { get; set; }
}