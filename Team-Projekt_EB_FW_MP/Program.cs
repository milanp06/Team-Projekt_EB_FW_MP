using Team_Projekt_EB_FW_MP.Components;
// SPAA: für Authorization
using Microsoft.AspNetCore.Components.Authorization;
using Team_Projekt_EB_FW_MP;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// SPAA: Service für Authorization
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<MyCustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
  provider.GetRequiredService<MyCustomAuthStateProvider>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();