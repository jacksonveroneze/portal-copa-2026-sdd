using Microsoft.EntityFrameworkCore;
using PortalCopa26.Data;
using PortalCopa26.Models;
using PortalCopa26.Services.Interfaces;

namespace PortalCopa26.Services;

public class SelecaoService(AppDbContext context) : ISelecaoService
{
    public async Task<IEnumerable<Selecao>> ListarSelecoesAsync()
        => await context.Selecoes
            .Include(s => s.Grupo)
            .OrderBy(s => s.Nome)
            .ToListAsync();

    public async Task<Selecao?> ObterPorIdAsync(int id)
        => await context.Selecoes
            .Include(s => s.Grupo)
            .Include(s => s.Jogadores)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<Selecao>> ListarPorGrupoAsync(int grupoId)
        => await context.Selecoes
            .Where(s => s.GrupoId == grupoId)
            .OrderBy(s => s.Nome)
            .ToListAsync();
}
