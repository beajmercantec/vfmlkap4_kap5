using Chatbot.Models;
using Chatbot.NLU;
using System.Collections.Generic;

namespace Chatbot.DM
{
    public class DialogManager
    {
        private readonly INluEngine _nlu;
        private readonly Dictionary<string, SessionState> _sessions = new();

        public DialogManager(INluEngine nlu)
        {
            _nlu = nlu;
        }

        public SessionState HandleIntent(Chatbot.NLU.NluResult nluResult, string sessionId)
        {
             if (!_sessions.ContainsKey(sessionId))
            {
                _sessions[sessionId] = new SessionState
                {
                    CurrentIntent = nluResult.Intent,
                    CollectedEntities = new Dictionary<string, string>(nluResult.Entities),
                    CurrentStep = "Start"
                };
            }

            var state = _sessions[sessionId];

            // Opdatér intent hvis det ikke er sat endnu
            //state.CurrentIntent ??= nluResult.Intent;
             state.CurrentIntent ??= nluResult.Intent ?? "BookRoom";

            // Tillad override af intent hvis brugeren skifter mening
            if (nluResult.Intent != null && nluResult.Intent != state.CurrentIntent)
            {
                Console.WriteLine($"Intent ændret fra {state.CurrentIntent} til {nluResult.Intent}");
                state.CurrentIntent = nluResult.Intent;
                state.CurrentStep = "Start";
                state.CollectedEntities.Clear();
            }
            // Opdatér entities (uden at overskrive eksisterende)
            foreach (var kvp in nluResult.Entities)
            {
                if (!state.CollectedEntities.ContainsKey(kvp.Key))
                {
                    state.CollectedEntities[kvp.Key] = kvp.Value;
                }
            }

            return state.CurrentIntent switch
            {
                "BookRoom" => HandleBooking(state),
                "CancelBooking" => HandleCancel(state),
                _ => HandleUnknown(state)
            };
        }

        private SessionState HandleBooking(SessionState state)
        {
            var step = state.CurrentStep;

            if (step == "Start")
            {
                state.CurrentStep = "AskCity";
                Console.WriteLine("Booking started, asking for city.");
            }
            else if (step == "AskCity" && state.CollectedEntities.ContainsKey("City"))
            {
                state.CurrentStep = "AskDate";
                Console.WriteLine("City received, asking for date.");
            }
            else if (step == "AskDate" &&
                    state.CollectedEntities.ContainsKey("FromDate") &&
                    state.CollectedEntities.ContainsKey("ToDate"))
            {
                state.CurrentStep = "AskGuests";
            }
            else if (step == "AskGuests" && state.CollectedEntities.ContainsKey("Guests"))
            {
                state.CurrentStep = "Confirm";
                Console.WriteLine("Guests received, confirming booking.");
            }

            return state;
        }

        private SessionState HandleCancel(SessionState state)
        {
            var step = state.CurrentStep;

            if (step == "Start")
            {
                state.CurrentStep = "AskBookingId";
            }
            else if (step == "AskBookingId" && state.CollectedEntities.ContainsKey("BookingId"))
            {
                state.CurrentStep = "ConfirmCancel";
            }

            return state;
        }

        private SessionState HandleUnknown(SessionState state)
        {
            state.CurrentStep = "Unknown";
            return state;
        }
    }
}