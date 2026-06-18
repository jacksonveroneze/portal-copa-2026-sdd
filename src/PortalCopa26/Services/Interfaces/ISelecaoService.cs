using PortalCopa26.Models;

namespace PortalCopa26.Services.Interfaces;

public interface ISelecaoService
{
    Task<IEnumerable<Selecao>> ListarSelecoesAsync();
    Task<Selecao?> ObterPorIdAsync(int id);
    Task<IEnumerable<Selecao>> ListarPorGrupoAsync(int grupoId);
}
