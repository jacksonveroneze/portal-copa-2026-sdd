using PortalCopa26.Models;

namespace PortalCopa26.Services.Interfaces;

public interface IJogoService
{
    Task<IEnumerable<Jogo>> ListarJogosAsync();
    Task<IEnumerable<Jogo>> ListarPorGrupoAsync(int grupoId);
    Task<IEnumerable<Jogo>> ListarOrdenadosPorDataAsync();
    Task<Jogo?> ObterPorIdAsync(int id);
}
