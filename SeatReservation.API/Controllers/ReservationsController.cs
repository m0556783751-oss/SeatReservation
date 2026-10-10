using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetById(
            [FromRoute] int id, CancellationToken ct)
        {
            var reservation = await _reservationService.GetByIdAsync(id, ct);
            return Ok(reservation);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ReservationDto>>> GetMyReservations(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var userId = GetUserId();
            var result = await _reservationService.GetByUserAsync(userId, page, pageSize, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Create(
            [FromBody] CreateReservationRequest request, CancellationToken ct)
        {
            var userId = GetUserId();
            var reservation = await _reservationService.CreateAsync(userId, request, ct);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel([FromRoute] int id, CancellationToken ct)
        {
            var userId = GetUserId();
            await _reservationService.CancelAsync(id, userId, ct);
            return NoContent();
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim!.Value);
        }
    }
}
