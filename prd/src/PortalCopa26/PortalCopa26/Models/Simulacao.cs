namespace PortalCopa26.Models;

public class Simulacao
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }

    public ICollection<SimulacaoJogo> SimulacaoJogos { get; set; } = [];
}
