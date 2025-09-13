// ChatService.cs – Orkestrerer: NLU → DM → NLG → Session

using Chatbot.NLU;
using Chatbot.DM;
using Chatbot.NLG;
using Chatbot.Models;

namespace BlazorUI.Services
{
    public class ChatService
    {
        private readonly INluEngine _nlu;
        private readonly DialogManager _dm;
        private readonly INlgEngine _nlg;
        private readonly ChatSession _session;
   
        public ChatService(INluEngine nlu, DialogManager dm, INlgEngine nlg, ChatSession session)
        {
            _nlu = nlu;
            _dm = dm;
            _nlg = nlg;
            _session = session;
        }

        public async Task<string> HandleUserInput(string input)
        {
          
            // 1. Tilføj brugerens besked til session
            _session.Messages.Add(new ChatMessage { Text = input, IsUser = true });

            // 2. Kør NLU på input
            Chatbot.NLU.NluResult nluResult = _nlu.Predict(input); 

            // 3. Send nluResult videre til DM (inkl. session-id som string)
            var state = _dm.HandleIntent(nluResult, _session.SessionId.ToString());

            // 4. Generér svar via NLG
            var response = _nlg.GenerateResponse(state);

            // 5. Tilføj svar til session
            _session.Messages.Add(new ChatMessage { Text = response, IsUser = false });

            return await Task.FromResult(response);
        }
    }
}