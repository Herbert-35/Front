using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrontAPI.Data;
using FrontAPI.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace FrontAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        // GET: api/Cities/?pageIndex=0&pageSize=10
        // GET: api/Cities/?pageIndex=0&pageSize=10&sortColumn=name&
        // sortOrder=asc
        [HttpGet]
        public async Task<ActionResult<ApiResult<LocationDTO>>> GetLocations(
            int pageIndex = 0, int pageSize = 10, string? sortColumn = null,
            string? sortOrder = null, string? filterColumn = null,
            string? filterQuery = null)
        {
            return await ApiResult<LocationDTO>.CreateAsync(
                _context.Locations.AsNoTracking()
                .Select(s => new LocationDTO()
                {
                    Id = s.Id,
                    Name = s.Name,
                    ZipCode = s.ZipCode,
                    StreetAddress = s.StreetAddress,
                    StateId = s.State!.Id,
                    StateName = s.State!.Name
                }),
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterColumn,
                filterQuery);
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
          if (_context.Locations == null)
          {
              return NotFound();
          }
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        [Authorize(Roles = "RegisteredUser")]
        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        [Authorize(Roles = "RegisteredUser")]
        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
          if (_context.Locations == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Locations'  is null.");
          }
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = location.Id }, location);
        }

        [Authorize(Roles = "Administrator")]
        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            if (_context.Locations == null)
            {
                return NotFound();
            }
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationExists(int id)
        {
            return (_context.Locations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        [Route("IsDupeLocation")]
        public bool IsDupeLocation(Location location)
        {
            return _context.Locations.Any(
            e => e.Name == location.Name
            && e.ZipCode == location.ZipCode
            && e.StreetAddress == location.StreetAddress
            && e.StateId == location.StateId
            && e.Id != location.Id
            );
        }
    }
}
