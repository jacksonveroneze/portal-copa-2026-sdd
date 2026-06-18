namespace PortalCopa26.Models;

public class SimulacaoJogo
{
    public int Id { get; set; }
    public int SimulacaoId { get; set; }
    public int JogoId { get; set; }
    public int? GolsSelecaoA { get; set; }
    public int? GolsSelecaoB { get; set; }

    public Simulacao Simulacao { get; set; } = null!;
    public Jogo Jogo { get; set; } = null!;
}
