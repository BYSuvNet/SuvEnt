using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventHomepage.Models;
using EventHomepage.ViewModels;
using EventInfrastructure.Data;
using EventCore.Exceptions;

namespace EventHomepage.Controllers;

public class HomeController : Controller
{
    private readonly EventDbContext _context;

    public HomeController(EventDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var events = await _context.Events
            .Select(e => new EventListViewModel
            {
                Id = e.Id,
                Name = e.Name,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime,
                Location = e.Location,
                RegistrationCount = e.Registrations.Count,
                MaxParticipants = e.MaxParticipants,
                IsFull = e.MaxParticipants.HasValue && e.Registrations.Count >= e.MaxParticipants.Value
            })
            .OrderBy(e => e.StartDateTime)
            .ToListAsync();

        return View(events);
    }

    public async Task<IActionResult> Details(int id)
    {
        var evt = await _context.Events
            .Where(e => e.Id == id)
            .Select(e => new EventDetailsViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime,
                Location = e.Location,
                MaxParticipants = e.MaxParticipants,
                RegistrationCount = e.Registrations.Count,
                IsFull = e.MaxParticipants.HasValue && e.Registrations.Count >= e.MaxParticipants.Value
            })
            .FirstOrDefaultAsync();

        if (evt == null)
        {
            return NotFound();
        }

        return View(evt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(int id, RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var evt = await _context.Events
                .Where(e => e.Id == id)
                .Select(e => new EventDetailsViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    Location = e.Location,
                    MaxParticipants = e.MaxParticipants,
                    RegistrationCount = e.Registrations.Count,
                    IsFull = e.MaxParticipants.HasValue && e.Registrations.Count >= e.MaxParticipants.Value,
                    Registration = model
                })
                .FirstOrDefaultAsync();

            if (evt == null)
            {
                return NotFound();
            }

            return View("Details", evt);
        }

        try
        {
            // var eventEntity = await _context.Events.FindAsync(id); // Buggy! Doesnt match well with domain logic
            // ta tillbaka detta som en integrationstestningsövning senare, där vi kan visa på varför det är viktigt att ladda in relaterade data när
            // samt hur man kan upptäcka en bugg som denna
            var eventEntity = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            eventEntity.RegisterParticipant(model.ParticipantName, model.ParticipantEmail);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Registration successful!";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (EventIsFullException)
        {
            ModelState.AddModelError("", "This event is full. Registration is no longer available.");
        }
        catch (DuplicateRegistrationException)
        {
            ModelState.AddModelError("", "This email is already registered for this event.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        // If we got here, something failed, reload the details view
        var failedEvt = await _context.Events
            .Where(e => e.Id == id)
            .Select(e => new EventDetailsViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime,
                Location = e.Location,
                MaxParticipants = e.MaxParticipants,
                RegistrationCount = e.Registrations.Count,
                IsFull = e.MaxParticipants.HasValue && e.Registrations.Count >= e.MaxParticipants.Value,
                Registration = model
            })
            .FirstOrDefaultAsync();

        return View("Details", failedEvt);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
