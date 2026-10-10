using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.API.Controllers
{
    [ApiController]
    [Route("api/venues/{venueId}/seats")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetByVenue(
            [FromRoute] int venueId, CancellationToken ct)
        {
            var seats = await _seatService.GetByVenueAsync(venueId, ct);
            return Ok(seats);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetAvailable(
            [FromRoute] int venueId, [FromQuery] int eventId, CancellationToken ct)
        {
            var seats = await _seatService.GetAvailableForEventAsync(venueId, eventId, ct);
            return Ok(seats);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SeatDto>> Create(
            [FromRoute] int venueId, [FromBody] CreateSeatRequest request, CancellationToken ct)
        {
            var seat = await _seatService.CreateAsync(venueId, request, ct);
            return Created("", seat);
        }

        [HttpDelete("/api/seats/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            await _seatService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
