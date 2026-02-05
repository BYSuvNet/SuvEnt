using System.Diagnostics;
using EventHomepage.Models;
using EventCore.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventHomepage.Controllers;

public class HomeController : Controller
{
    
    public IActionResult Index()
    {
        List<Event> events = [
            new("SUVNET KRÖK OCH STÖK", "Stöka och kröka med valfria klasskamrater!", startDateTime: DateTime.Now.AddDays(10), endDateTime: DateTime.Now.AddDays(11), "YHB", 100),
            new("Testa Festa", "Vi testar festa och koda samtidigt hehe", startDateTime: DateTime.Now.AddDays(74), endDateTime: DateTime.Now.AddDays(75), "Södra Torget", 419)];
        
        return View(events);
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
