using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<EventDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? categoryId = null,
            CancellationToken ct = default)
        {
            var result = await _eventService.GetPagedAsync(page, pageSize, search, categoryId, ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetById([FromRoute] int id, CancellationToken ct)
        {
            var ev = await _eventService.GetByIdAsync(id, ct);
            return Ok(ev);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EventDto>> Create(
            [FromBody] CreateEventRequest request, CancellationToken ct)
        {
            var ev = await _eventService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = ev.Id }, ev);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EventDto>> Update(
            [FromRoute] int id, [FromBody] UpdateEventRequest request, CancellationToken ct)
        {
            var ev = await _eventService.UpdateAsync(id, request, ct);
            return Ok(ev);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
        {
            await _eventService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
