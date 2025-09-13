## Korrekt flow (SoC: Separation of Concerns)
UI → NLU → DM → NLG → UI
### Forklaring
	•	UI (Blazor) kalder kun ChatService.HandleUserInpu(userInput)
	•	BlazorUI/Services/ChatService:
	    •	Kalder NLU.Predict(userInput)
	    •	Sender NluResult + SessionId til DialogManager.HandleIntent(...)
	    •	Sender ny SessionState til NLG.GenerateResponse(state)
	    •	ChatSession holder styr på dialoghistorik og sessionState

### Opdeling i komponenter
* Komponent           Ansvar
* ChatService         Bindeled mellem UI og logik
* INluEngine          Forstår hvad brugeren mener (intent + entity)
* DialogManager       Styrer samtalen og flow via statemachine
* INlgEngine          Genererer svar (tekst output)

