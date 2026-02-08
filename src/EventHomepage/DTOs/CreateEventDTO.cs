namespace EventHomepage.DTOs;

public record CreateEventDTO(
    string Name,
    string Description,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Location,
    int? MaxParticipants
);

/*
CreateEventDTO = Input, används för att skicka data TILL servern
EventDTO = Output, avnänds för att skicka data FRÅN servern och innehåller ALLA fält
Jag använder dock inte API i min MVC(HomeController) utom jag använder ist DbContext direkt i MVC:n

För den här gången!..
*/