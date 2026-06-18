namespace PortalCopa26.Models;

public class Jogo
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public int SelecaoAId { get; set; }
    public int SelecaoBId { get; set; }
    public int? GrupoId { get; set; }
    public string Estadio { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Fase { get; set; } = string.Empty;
    public int? GolsSelecaoA { get; set; }
    public int? GolsSelecaoB { get; set; }

    public Selecao SelecaoA { get; set; } = null!;
    public Selecao SelecaoB { get; set; } = null!;
    public Grupo? Grupo { get; set; }
}
