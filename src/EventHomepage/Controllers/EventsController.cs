using EventCore.Entities;
using EventHomepage.DTOs;
using EventInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHomepage.Controllers;

/*
Skillnaden mellan MVC och API Controllers är att 
MVC Controller används för att rendera HTML-sidor (Views), hantera request från webbläsaren och att returnera View(), RedirectToAction() osv. 
API Controller används för att returnera data (JSON) och skapa REST API endpoints.
Därför måste jag ha två olika controllers.

ControllerBase är en inbyggd klass i ASP.NER Core
*/


[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly EventDbContext _context;
    
    public EventsController(EventDbContext context)
    {
        _context = context;
    }
    
    [HttpPost] // POST: skapa ett nytt event
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
    {
        // API behöver egen validering, Event.cs har en domänvalidering som skyddar affärslogiken och castar ArgumentException
        // API valideringar returnerar dock statuskoder med lite mer detaljerad information till KLIENTEN
        if (dto.EndDateTime <= dto.StartDateTime)
            return BadRequest("EndDateTime must be after StartDateTime");
            
        // skapa nytt Event via domänkonstruktorn, eftersom Event är min domänmodell
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
        
        // skapa ett DTO som returneras till klienten, CreateEventDTO är bara ren data som serialiseras till JASON
        var dtoResponse = new CreateEventDTO(
            
            newEvent.Name,
            newEvent.Description,
            newEvent.StartDateTime,
            newEvent.EndDateTime,
            newEvent.Location,
            newEvent.MaxParticipants
            
        );
        
        // returnera statuskod 201 Created tillsammans med hela eventet
        return Created($"/api/events/{newEvent.Id}", dtoResponse);
    }
    
    [HttpGet] // GET: hämta ALLA events
    public async Task<IActionResult> GetAllEvents()
    {
        var allEvents = await _context.Events.Include(e => e.Registrations)
        .ToListAsync();
        
        return Ok(allEvents);
    }
    
    
}