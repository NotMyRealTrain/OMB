using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMB.Api.Data;
using OMB.Api.DTOs;
using OMB.Api.Models;

namespace OMB.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResidentsController : ControllerBase
{
    private readonly OmbDbContext _context;

    public ResidentsController(OmbDbContext context)
    {
        _context = context;
    }

    // alle bewoners ophalen, inclusief hun locatie
    [HttpGet]
    public async Task<IActionResult> GetResidents()
    {
        var residents = await _context.Residents
            .OrderBy(r => r.FirstName)
            .Select(r => new GetResidentDto
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                LocationId = r.LocationId,
                IddsiLevel = r.IddsiLevel,
                IsVegetarian = r.IsVegetarian,
                AllergenNotes = r.AllergenNotes,
                IsActive = r.IsActive,
                UpdatedAt = r.UpdatedAt,
            })
            .ToListAsync();

        return Ok(residents);
    }

    // specifieke bewoner ophalen op ID, inclusief locatie
    [HttpGet("{id}")]
    public async Task<IActionResult> GetResident(long id)
    {
        var resident = await _context.Residents.FindAsync(id);

        if (resident == null)
        {
            return NotFound();
        }

        var result = new GetResidentDto
        {
            Id = resident.Id,
            FirstName = resident.FirstName,
            LastName = resident.LastName,
            LocationId = resident.LocationId,
            IddsiLevel = resident.IddsiLevel,
            IsVegetarian = resident.IsVegetarian,
            AllergenNotes = resident.AllergenNotes,
            IsActive = resident.IsActive,
            UpdatedAt = resident.UpdatedAt,
        };

        return Ok(result);
    }

    // bewoners zoeken op naam (voornaam + achternaam), case-insensitive en gedeeltelijke overeenkomsten toestaan
    [HttpGet("search")]
    public async Task<IActionResult> SearchResidents([FromQuery] string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Query parameter 'name' is required.");
        }

        var trimmedName = name.Trim();
        var pattern = $"%{trimmedName}%";
        var residents = await _context.Residents
            .Where(r => EF.Functions.ILike(r.FirstName + " " + r.LastName, pattern))
            .Select(r => new GetResidentDto
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                LocationId = r.LocationId,
                IddsiLevel = r.IddsiLevel,
                IsVegetarian = r.IsVegetarian,
                AllergenNotes = r.AllergenNotes,
                IsActive = r.IsActive,
                UpdatedAt = r.UpdatedAt,
            })
            .ToListAsync();

        return Ok(residents);
    }

    // nieuwe bewoner aanmaken
    [HttpPost]
    public async Task<IActionResult> CreateResident(CreateResidentDto dto)
    {
        var resident = new Resident
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            LocationId = dto.LocationId,
            IddsiLevel = dto.IddsiLevel,
            IsVegetarian = dto.IsVegetarian,
            AllergenNotes = dto.AllergenNotes ?? string.Empty,
            IsActive = dto.IsActive,
        };

        _context.Residents.Add(resident);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetResident), new { id = resident.Id }, new GetResidentDto
        {
            Id = resident.Id,
            FirstName = resident.FirstName,
            LastName = resident.LastName,
            LocationId = resident.LocationId,
            IddsiLevel = resident.IddsiLevel,
            IsVegetarian = resident.IsVegetarian,
            AllergenNotes = resident.AllergenNotes,
            IsActive = resident.IsActive,
        });
    }

    // bestaande bewoner bijwerken
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResident(long id, UpdateResidentDto dto)
    {
        var resident = await _context.Residents.FindAsync(id);

        if (resident == null)
        {
            return NotFound();
        }

        if (resident.LocationId != dto.LocationId)
        {
            var locationExists = await _context.Locations.AnyAsync(l => l.Id == dto.LocationId);
            if (!locationExists)
            {
                return BadRequest($"Location with ID {dto.LocationId} does not exist.");
            }
        }

        resident.FirstName = dto.FirstName;
        resident.LastName = dto.LastName;
        resident.LocationId = dto.LocationId;
        resident.IddsiLevel = dto.IddsiLevel;
        resident.IsVegetarian = dto.IsVegetarian;
        resident.AllergenNotes = dto.AllergenNotes ?? string.Empty;
        resident.IsActive = dto.IsActive;
        resident.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new GetResidentDto
        {
            Id = resident.Id,
            FirstName = resident.FirstName,
            LastName = resident.LastName,
            LocationId = resident.LocationId,
            IddsiLevel = resident.IddsiLevel,
            IsVegetarian = resident.IsVegetarian,
            AllergenNotes = resident.AllergenNotes,
            IsActive = resident.IsActive,
            UpdatedAt = resident.UpdatedAt,
        });
    }

    // bewoner verwijderen
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResident(long id)
    {
        var resident = await _context.Residents.FindAsync(id);

        if (resident == null)
        {
            return NotFound();
        }

        _context.Residents.Remove(resident);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}