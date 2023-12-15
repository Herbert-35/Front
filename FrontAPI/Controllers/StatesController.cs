using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FrontAPI.Data;
using FrontAPI.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace FrontAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<ApiResult<StateDTO>>> GetStates(
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
        {
            return await ApiResult <StateDTO>.CreateAsync(
            _context.States.AsNoTracking()
            .Select(s => new StateDTO()
            {
                Id = s.Id,
                Name = s.Name,
                RestaurantName = s.RestaurantName,
                PhoneNumber = s.PhoneNumber,
                Cuisine = s.Cuisine,
                totLocations = s.Locations!.Count
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        // GET: api/States/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
          if (_context.States == null)
          {
              return NotFound();
          }
            var state = await _context.States.FindAsync(id);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        [Authorize(Roles = "RegisteredUser")]
        // PUT: api/States/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, State state)
        {
            if (id != state.Id)
            {
                return BadRequest();
            }

            _context.Entry(state).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
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
        // POST: api/States
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
          if (_context.States == null)
          {
              return Problem("Entity set 'ApplicationDbContext.States'  is null.");
          }
            _context.States.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetState", new { id = state.Id }, state);
        }

        [Authorize(Roles = "Administrator")]
        // DELETE: api/States/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            if (_context.States == null)
            {
                return NotFound();
            }
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.States.Remove(state);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StateExists(int id)
        {
            return (_context.States?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        [Route("IsDupeField")]
        public bool IsDupeField(
        int countryId,
        double fieldD,
        string fieldName,
        string fieldValue)
        {
            switch (fieldName)
            {
                case "name":
                    return _context.States.Any(
                    c => c.Name == fieldValue && c.Id != countryId);
                case "restaurantName":
                    return _context.States.Any(
                    c => c.RestaurantName == fieldValue && c.Id != countryId);
                case "phoneNumber":
                    return _context.States.Any(
                    c => c.PhoneNumber == fieldD && c.Id != countryId);
                default:
                    return false;
            }
        }

    }
}
