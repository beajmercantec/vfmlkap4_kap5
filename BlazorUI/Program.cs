using BlazorUI.Components;
using BlazorUI.Services;

// 👇 Tilføj disse using-sætninger:
using Chatbot.NLU;
using Chatbot.DM;
using Chatbot.NLG;
using Chatbot.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// 👇 Tilføj alle dine afhængigheder her:
builder.Services.AddScoped<INluEngine, HybridNlu>();     // Hybrid NLU
builder.Services.AddScoped<INlgEngine, SimpleNlg>();  // NLG engine
builder.Services.AddScoped<DialogManager>();             // Dialog Manager
builder.Services.AddScoped<ChatSession>();               // Session tracking
builder.Services.AddScoped<ChatService>();               // Chat orkestrering

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();