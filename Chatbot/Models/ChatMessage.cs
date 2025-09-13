// TODO: Define chat message model
namespace Chatbot.Models;

public class ChatMessage
{
    public string Text { get; set; }
    public bool IsUser { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now; 
}