using System.Diagnostics;
using EventHomepage.Models;
using EventInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventHomepage.Controllers;

public class HomeController : Controller
{
    private readonly EventDbContext _context;

    public HomeController(EventDbContext context)
    {
        _context = context;
    }
    
    // gör om till asynkronsk för att läsa in från DB
    public async Task<IActionResult> Index()
    {
        // hämta events 
        var allEvents = await _context.Events.ToListAsync();
        return View(allEvents);
        
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


/* 
Följande routes skall hanteras av MVC:

Route                           Beskrivning
                            
/home/index	                    Visar en lista på kommande event
/home/events/{id}	            Visar detaljer för ett specifikt event
(POST) /home/register/{id}	    Tar emot anmälan för ett event
*/
