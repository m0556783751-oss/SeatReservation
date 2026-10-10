using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetAll(CancellationToken ct)
        {
            var venues = await _venueService.GetAllAsync(ct);
            return Ok(venues);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VenueDto>> GetById([FromRoute] int id, CancellationToken ct)
        {
            var venue = await _venueService.GetByIdAsync(id, ct);
            return Ok(venue);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<VenueDto>> Create(
            [FromBody] CreateVenueRequest request, CancellationToken ct)
        {
            var venue = await _venueService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = venue.Id }, venue);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<VenueDto>> Update(
            [FromRoute] int id, [FromBody] UpdateVenueRequest request, CancellationToken ct)
        {
            var venue = await _venueService.UpdateAsync(id, request, ct);
            return Ok(venue);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            await _venueService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
