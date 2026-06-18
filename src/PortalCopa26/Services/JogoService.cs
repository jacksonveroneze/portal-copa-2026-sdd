using Microsoft.EntityFrameworkCore;
using PortalCopa26.Data;
using PortalCopa26.Models;
using PortalCopa26.Services.Interfaces;

namespace PortalCopa26.Services;

public class JogoService(AppDbContext context) : IJogoService
{
    public async Task<IEnumerable<Jogo>> ListarJogosAsync()
        => await context.Jogos
            .Include(j => j.SelecaoA)
            .Include(j => j.SelecaoB)
            .Include(j => j.Grupo)
            .ToListAsync();

    public async Task<IEnumerable<Jogo>> ListarPorGrupoAsync(int grupoId)
        => await context.Jogos
            .Include(j => j.SelecaoA)
            .Include(j => j.SelecaoB)
            .Where(j => j.GrupoId == grupoId)
            .OrderBy(j => j.DataHora)
            .ToListAsync();

    public async Task<IEnumerable<Jogo>> ListarOrdenadosPorDataAsync()
        => await context.Jogos
            .Include(j => j.SelecaoA)
            .Include(j => j.SelecaoB)
            .Include(j => j.Grupo)
            .OrderBy(j => j.DataHora)
            .ToListAsync();

    public async Task<Jogo?> ObterPorIdAsync(int id)
        => await context.Jogos
            .Include(j => j.SelecaoA)
            .Include(j => j.SelecaoB)
            .Include(j => j.Grupo)
            .FirstOrDefaultAsync(j => j.Id == id);
}
