namespace PortalCopa26.Models;

public class Jogador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Posicao { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public int GolsMarcados { get; set; }
    public int ParticipacoesCopa { get; set; }
    public int SelecaoId { get; set; }

    public Selecao Selecao { get; set; } = null!;

    public int Idade => CalcularIdade();

    private int CalcularIdade()
    {
        var hoje = DateOnly.FromDateTime(DateTime.Today);
        var idade = hoje.Year - DataNascimento.Year;
        if (DataNascimento > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}
