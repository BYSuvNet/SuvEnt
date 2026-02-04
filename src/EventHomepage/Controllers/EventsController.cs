using EventCore.Entities;
using EventHomepage.DTOs;
using EventInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHomepage.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(EventDbContext context) : ControllerBase
{
    private readonly EventDbContext _context = context;

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto dto)
    {
        try
        {
            var newEvent = new Event(
                dto.Name,
                dto.Description,
                dto.StartDateTime,
                dto.EndDateTime,
                dto.Location,
                dto.MaxParticipants
            );

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            var eventDto = new EventDto(
                newEvent.Id,
                newEvent.Name,
                newEvent.Description,
                newEvent.StartDateTime,
                newEvent.EndDateTime,
                newEvent.Location,
                newEvent.MaxParticipants,
                newEvent.Registrations.Count,
                newEvent.IsFull()
            );

            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, eventDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        // var evt = await _context.Events.FindAsync(id);
        var evt = await _context.Events.Include(e => e.Registrations)
                                       .FirstOrDefaultAsync(e => e.Id == id);

        if (evt == null)
        {
            return NotFound();
        }

        var eventDto = new EventDto(
            evt.Id,
            evt.Name,
            evt.Description,
            evt.StartDateTime,
            evt.EndDateTime,
            evt.Location,
            evt.MaxParticipants,
            evt.Registrations.Count,
            evt.IsFull()
        );

        return eventDto;
    }
}
