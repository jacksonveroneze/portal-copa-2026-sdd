namespace PortalCopa26.Models;

public class Selecao
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoFifa { get; set; } = string.Empty;
    public int GrupoId { get; set; }
    public int PontuacaoRanking { get; set; }
    public int PosicaoRanking { get; set; }

    public Grupo Grupo { get; set; } = null!;
    public ICollection<Jogador> Jogadores { get; set; } = [];
    public ICollection<Jogo> JogosComoSelecaoA { get; set; } = [];
    public ICollection<Jogo> JogosComoSelecaoB { get; set; } = [];
    public RankingFifa? RankingFifa { get; set; }
}
