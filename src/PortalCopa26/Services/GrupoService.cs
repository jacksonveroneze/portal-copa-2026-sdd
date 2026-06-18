using Microsoft.EntityFrameworkCore;
using PortalCopa26.Data;
using PortalCopa26.Models;
using PortalCopa26.Services.Interfaces;

namespace PortalCopa26.Services;

public class GrupoService(AppDbContext context) : IGrupoService
{
    public async Task<IEnumerable<Grupo>> ListarGruposAsync()
        => await context.Grupos
            .Include(g => g.Selecoes)
            .OrderBy(g => g.Letra)
            .ToListAsync();

    public async Task<Grupo?> ObterPorIdAsync(int id)
        => await context.Grupos
            .Include(g => g.Selecoes)
            .FirstOrDefaultAsync(g => g.Id == id);
}
