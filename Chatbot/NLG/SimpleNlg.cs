using Chatbot.Models;
using System.Linq;

namespace Chatbot.NLG;

public class SimpleNlg : INlgEngine
{
    public string GenerateResponse(SessionState state)
    {
        var step = state.CurrentStep;
        var entities = state.CollectedEntities;

        // Entity fallbacks
        entities.TryGetValue("City", out var city);
        city ??= "byen";

        entities.TryGetValue("Date", out var date);
        date ??= "en ukendt dato";

        entities.TryGetValue("Guests", out var guests);
        guests ??= "1";

        entities.TryGetValue("BookingId", out var bookingId);
        bookingId ??= "ukendt ID";

        return state.CurrentIntent switch
        {
            "BookRoom" => step switch
            {
                "AskCity" => "Hvilken by vil du bo i?",
                "AskDate" => $"Hvornår ønsker du at bo i {city}?",
                "AskGuests" => "Hvor mange gæster skal der være?",
                "Confirm" => $"Tak! Jeg har reserveret et værelse i {city} den {date} for {guests} gæst(er).",
                _ => "Jeg forstod ikke din forespørgsel."
            },
            "CancelBooking" => step switch
            {
                "AskBookingId" => "Hvad er ID'et på din booking, du vil aflyse?",
                "ConfirmCancel" => $"Din booking med ID {bookingId} er nu aflyst.",
                _ => "Beklager, noget gik galt."
            },
            _ => "Beklager, jeg forstod ikke hvad du mente."
        };
    }
}