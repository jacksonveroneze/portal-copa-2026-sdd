namespace PortalCopa26.Models;

public class RankingFifa
{
    public int Id { get; set; }
    public int SelecaoId { get; set; }
    public int Posicao { get; set; }
    public double Pontuacao { get; set; }
    public DateTime DataAtualizacao { get; set; }

    public Selecao Selecao { get; set; } = null!;
}
