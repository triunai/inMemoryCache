using ApiCachingApp.Data;
using ApiCachingApp.Models;
using ApiCachingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCachingApp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DriversController : ControllerBase{
    private readonly ApiDbContext _context;
    private readonly ICacheService _cacheService;
    public DriversController(
        ApiDbContext context,
        ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    // GET: api/Drivers
    [HttpGet]
    public async Task<IActionResult> GetDrivers()
{
    var cachedDrivers = _cacheService.GetData<IEnumerable<Driver>>("drivers");

    if (cachedDrivers != null && cachedDrivers.Any())
    {
        // If there are cached drivers, return them
        return Ok(cachedDrivers);
    }
    else
    {
        // If not, fetch from the database and cache them
        var dbDrivers = await _context.Drivers.ToListAsync();
        var expiryTime = DateTimeOffset.Now.AddMinutes(2);

        _cacheService.setData<IEnumerable<Driver>>("drivers", dbDrivers, expiryTime);
        
        return Ok(dbDrivers);
    }
}


    // GET: api/Drivers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Driver>> GetDriver(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);

        if (driver == null)
        {
            return NotFound();
        }

        return driver;
    }
    // POST: api/driver
    [HttpPost]
    public async Task<ActionResult<Driver>> CreateDriver(Driver driver)
    {
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();

        // returns 201
        return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
    }

    // PUT: api/driver/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDriver(int id, Driver driver)
    {
        if (id != driver.Id)
        {
            return BadRequest();
        }

        _context.Entry(driver).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DriverExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/driver/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null)
        {
            return NotFound();
        }

        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DriverExists(int id)
    {
        return _context.Drivers.Any(e => e.Id == id);
    }
}

