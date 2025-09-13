namespace Chatbot.NLU;
using System.Collections.Generic;

/**
 *(typisk ligger denne i Models, men kan godt være i NLU)
 * Return-type for Predict-metoden i INluEngine
 * Intent → fx "BookRoom"
 * Entities → fx {"City": "Aarhus", "Guests": "2"}
 * Det er den struktur DM kan arbejde videre med.
 */
public class NluResult
{
    public string Intent { get; set; }
    public Dictionary<string, string> Entities { get; set; }

    public NluResult(string intent, Dictionary<string, string> entities = null)
    {
        Intent = intent;
        Entities = entities ?? new Dictionary<string, string>();
    }
}