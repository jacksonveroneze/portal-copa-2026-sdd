using Microsoft.EntityFrameworkCore;
using PortalCopa26.Data;
using PortalCopa26.Models;
using PortalCopa26.Services.Interfaces;

namespace PortalCopa26.Services;

public class RankingFifaService(AppDbContext context) : IRankingFifaService
{
    public async Task<IEnumerable<RankingFifa>> ObterRankingOrdenadoAsync()
        => await context.RankingFifa
            .Include(r => r.Selecao)
            .OrderBy(r => r.Posicao)
            .ToListAsync();
}
