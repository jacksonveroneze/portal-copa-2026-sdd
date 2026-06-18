using PortalCopa26.Models;

namespace PortalCopa26.Services.Interfaces;

public interface IRankingFifaService
{
    Task<IEnumerable<RankingFifa>> ObterRankingOrdenadoAsync();
}
