using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMB.Api.Data;
using OMB.Api.DTOs;
using OMB.Api.Models;

namespace OMB.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly OmbDbContext _context;

    public LocationsController(OmbDbContext context)
    {
        _context = context;
    }

    // alle locaties ophalen
    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _context.Locations
            .OrderBy(l => l.Name)
            .Select(l => new GetLocationDto
            {
                Id = l.Id,
                Name = l.Name,
                Code = l.Code,
                Active = l.Active,
            })
            .ToListAsync();

        return Ok(locations);
    }

    // locatie op id ophalen
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLocation(long id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        var result = new GetLocationDto
        {
            Id = location.Id,
            Name = location.Name,
            Code = location.Code,
            Active = location.Active,
        };

        return Ok(result);
    }

    // locaties zoeken op naam
    [HttpGet("search")]
    public async Task<IActionResult> SearchLocations(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Query parameter 'name' is required.");
        }

        var normalizedName = name.Trim();

        var locations = await _context.Locations
            .Where(l => EF.Functions.ILike(l.Name, $"%{normalizedName}%"))
            .OrderBy(l => l.Name)
            .Select(l => new GetLocationDto
            {
                Id = l.Id,
                Name = l.Name,
                Code = l.Code,
                Active = l.Active,
            })
            .ToListAsync();

        return Ok(locations);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation(CreateLocationDto dto)
    {
        var location = new Location
        {
            Name = dto.Name,
            Code = dto.Code,
            Active = dto.Active,
        };

        _context.Locations.Add(location);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, new GetLocationDto
        {
            Id = location.Id,
            Name = location.Name,
            Code = location.Code,
            Active = location.Active,
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(long id, UpdateLocationDto dto)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        location.Name = dto.Name;
        location.Code = dto.Code;
        location.Active = dto.Active;

        await _context.SaveChangesAsync();

        return Ok(new GetLocationDto
        {
            Id = location.Id,
            Name = location.Name,
            Code = location.Code,
            Active = location.Active,
        });
    }

    // locatie verwijderen
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(long id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        _context.Locations.Remove(location);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}