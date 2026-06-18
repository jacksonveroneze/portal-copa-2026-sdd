using Microsoft.EntityFrameworkCore;
using PortalCopa26.Components;
using PortalCopa26.Data;
using PortalCopa26.Services;
using PortalCopa26.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGrupoService, GrupoService>();
builder.Services.AddScoped<ISelecaoService, SelecaoService>();
builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<IRankingFifaService, RankingFifaService>();

builder.Services.AddHostedService<DatabaseSeeder>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>();

app.Run();
