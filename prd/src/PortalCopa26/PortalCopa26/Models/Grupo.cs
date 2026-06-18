namespace PortalCopa26.Models;

public class Grupo
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Letra { get; set; } = string.Empty;

    public ICollection<Selecao> Selecoes { get; set; } = [];
    public ICollection<Jogo> Jogos { get; set; } = [];
}
