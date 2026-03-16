using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMB.Api.Data;
using OMB.Api.DTOs;
using OMB.Api.Models;

namespace OMB.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OmbDbContext _context;

    public OrdersController(OmbDbContext context)
    {
        _context = context;
    }

    

}