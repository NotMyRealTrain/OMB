using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMB.Api.Data;

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

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _context.Locations
            .OrderBy(l => l.Name)
            .ToListAsync();

        return Ok(locations);
    }
}