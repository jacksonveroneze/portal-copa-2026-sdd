using PortalCopa26.Models;

namespace PortalCopa26.Services.Interfaces;

public interface IGrupoService
{
    Task<IEnumerable<Grupo>> ListarGruposAsync();
    Task<Grupo?> ObterPorIdAsync(int id);
}
