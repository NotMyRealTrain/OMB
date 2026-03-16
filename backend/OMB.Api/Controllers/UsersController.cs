using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMB.Api.Data;
using OMB.Api.DTOs;
using OMB.Api.Models;

namespace OMB.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly OmbDbContext _context;

    public UsersController(OmbDbContext context)
    {
        _context = context;
    }


    // alle users ophalen, inclusief hun rollen en locaties
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.DefaultLocation)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Locations)
            .OrderBy(u => u.DisplayNameLast)
            .ThenBy(u => u.DisplayNameFirst)
            .Select(u => new GetUserDto
            {
                Id = u.Id,
                Email = u.Email,
                DisplayNameFirst = u.DisplayNameFirst,
                DisplayNameLast = u.DisplayNameLast,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                DefaultLocationId = u.DefaultLocationId,
                DefaultLocationName = u.DefaultLocation != null ? u.DefaultLocation.Name : null,
                Roles = u.UserRoles
                    .Select(ur => ur.Role.Name.ToString())
                    .ToList(),
                Locations = u.Locations
                    .Select(l => new LocationDto
                    {
                        Id = l.Id,
                        Name = l.Name
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(users);
    }

    // specifieke user ophalen op id, inclusief rollen en locaties
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(long id)
    {
        var user = await _context.Users
            .Include(u => u.DefaultLocation)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Locations)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var result = new GetUserDto
        {
            Id = user.Id,
            Email = user.Email,
            DisplayNameFirst = user.DisplayNameFirst,
            DisplayNameLast = user.DisplayNameLast,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            DefaultLocationId = user.DefaultLocationId,
            DefaultLocationName = user.DefaultLocation != null ? user.DefaultLocation.Name : null,
            Roles = user.UserRoles
                .Select(ur => ur.Role.Name.ToString())
                .ToList(),
            Locations = user.Locations
                .Select(l => new LocationDto
                {
                    Id = l.Id,
                    Name = l.Name
                })
                .ToList()
        };
        return Ok(result);
    }

    // user updaten, inclusief validatie dat default location een van de assigned locations is
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(long id, UpdateUserDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Locations)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        if (dto.DefaultLocationId.HasValue &&
            !user.Locations.Any(l => l.Id == dto.DefaultLocationId.Value))
        {
            return BadRequest("Default location must be one of the user's assigned locations.");
        }

        user.Email = dto.Email;
        user.DisplayNameFirst = dto.DisplayNameFirst;
        user.DisplayNameLast = dto.DisplayNameLast;
        user.DefaultLocationId = dto.DefaultLocationId;
        user.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // locatie toevoegen aan user
    [HttpPost("{id}/locations")]
    public async Task<IActionResult> AddLocationToUser(long id, AddUserLocationDto dto)
    {
        var user = await _context.Users
            .Include(u => u.Locations)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        var location = await _context.Locations.FindAsync(dto.LocationId);

        if (location == null)
        {
            return NotFound("Location not found.");
        }

        if (user.Locations.Any(l => l.Id == dto.LocationId))
        {
            return BadRequest("Location is already assigned to this user.");
        }

        user.Locations.Add(location);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}/locations/{locationId}")]
    public async Task<IActionResult> RemoveLocationFromUser(long id, long locationId)
    {
        var user = await _context.Users
            .Include(u => u.Locations)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        var location = user.Locations.FirstOrDefault(l => l.Id == locationId);

        if (location == null)
        {
            return NotFound("Location is not assigned to this user.");
        }

        if (user.DefaultLocationId == locationId)
        {
            user.DefaultLocationId = null;
        }

        user.Locations.Remove(location);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}