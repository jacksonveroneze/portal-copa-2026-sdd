using Microsoft.EntityFrameworkCore;
using PortalCopa26.Models;

namespace PortalCopa26.Data;

public class DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (await context.Selecoes.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Banco de dados já populado. Seed ignorado.");
            return;
        }

        logger.LogInformation("Iniciando seed do banco de dados...");

        var grupos = SeedGrupos(context);
        await context.SaveChangesAsync(cancellationToken);

        var selecoes = SeedSelecoes(context, grupos);
        await context.SaveChangesAsync(cancellationToken);

        SeedJogadores(context, selecoes);
        await context.SaveChangesAsync(cancellationToken);

        SeedJogos(context, selecoes, grupos);
        await context.SaveChangesAsync(cancellationToken);

        SeedRankingFifa(context, selecoes);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seed concluído com sucesso.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    // ──────────────────────────────────────────
    // GRUPOS
    // ──────────────────────────────────────────

    private static Dictionary<string, Grupo> SeedGrupos(AppDbContext context)
    {
        var letras = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
        var grupos = letras.Select(l => new Grupo { Letra = l, Nome = $"Grupo {l}" }).ToList();
        context.Grupos.AddRange(grupos);
        return grupos.ToDictionary(g => g.Letra);
    }

    // ──────────────────────────────────────────
    // SELEÇÕES
    // ──────────────────────────────────────────

    private static Dictionary<string, Selecao> SeedSelecoes(AppDbContext context, Dictionary<string, Grupo> grupos)
    {
        // (Nome, CodigoFifa, PosicaoRanking, PontuacaoRanking, LetraGrupo)
        var dados = new (string Nome, string Codigo, int Pos, int Pts, string Grupo)[]
        {
            // Grupo A
            ("Estados Unidos",    "us",     13, 1703, "A"),
            ("Inglaterra",        "gb-eng",  3, 1816, "A"),
            ("Venezuela",         "ve",     35, 1548, "A"),
            ("Camarões",          "cm",     31, 1574, "A"),
            // Grupo B
            ("México",            "mx",     19, 1659, "B"),
            ("Polônia",           "pl",     30, 1588, "B"),
            ("Paraguai",          "py",     27, 1598, "B"),
            ("Tunísia",           "tn",     29, 1592, "B"),
            // Grupo C
            ("Canadá",            "ca",     43, 1513, "C"),
            ("Croácia",           "hr",     14, 1697, "C"),
            ("Equador",           "ec",     23, 1641, "C"),
            ("Marrocos",          "ma",     11, 1712, "C"),
            // Grupo D
            ("França",            "fr",      2, 1839, "D"),
            ("Sérvia",            "rs",     22, 1643, "D"),
            ("Argentina",         "ar",      1, 1887, "D"),
            ("Japão",             "jp",     12, 1714, "D"),
            // Grupo E
            ("Espanha",           "es",      8, 1763, "E"),
            ("Turquia",           "tr",     25, 1616, "E"),
            ("Colômbia",          "co",      9, 1760, "E"),
            ("África do Sul",     "za",     60, 1452, "E"),
            // Grupo F
            ("Alemanha",          "de",     14, 1699, "F"),
            ("Suíça",             "ch",     16, 1680, "F"),
            ("Brasil",            "br",      5, 1784, "F"),
            ("Argélia",           "dz",     36, 1543, "F"),
            // Grupo G
            ("Portugal",          "pt",      6, 1779, "G"),
            ("Escócia",           "gb-sco", 37, 1536, "G"),
            ("Uruguai",           "uy",     15, 1693, "G"),
            ("Coreia do Sul",     "kr",     17, 1675, "G"),
            // Grupo H
            ("Holanda",           "nl",      7, 1770, "H"),
            ("Irã",               "ir",     26, 1610, "H"),
            ("Senegal",           "sn",     18, 1663, "H"),
            ("Honduras",          "hn",     77, 1397, "H"),
            // Grupo I
            ("Bélgica",           "be",      4, 1790, "I"),
            ("Austrália",         "au",     24, 1625, "I"),
            ("Egito",             "eg",     40, 1523, "I"),
            ("Costa Rica",        "cr",     50, 1490, "I"),
            // Grupo J
            ("Itália",            "it",     10, 1730, "J"),
            ("Arábia Saudita",    "sa",     58, 1461, "J"),
            ("Nigéria",           "ng",     28, 1597, "J"),
            ("Panamá",            "pa",     52, 1483, "J"),
            // Grupo K
            ("Áustria",           "at",     20, 1656, "K"),
            ("Catar",             "qa",     37, 1538, "K"),
            ("Costa do Marfim",   "ci",     39, 1527, "K"),
            ("Nova Zelândia",     "nz",     97, 1300, "K"),
            // Grupo L
            ("Dinamarca",         "dk",     21, 1648, "L"),
            ("Iraque",            "iq",     55, 1472, "L"),
            ("Uzbequistão",       "uz",     65, 1438, "L"),
            ("Jordânia",          "jo",     66, 1434, "L"),
        };

        var selecoes = dados.Select(d => new Selecao
        {
            Nome = d.Nome,
            CodigoFifa = d.Codigo,
            PosicaoRanking = d.Pos,
            PontuacaoRanking = d.Pts,
            GrupoId = grupos[d.Grupo].Id
        }).ToList();

        context.Selecoes.AddRange(selecoes);
        return selecoes.ToDictionary(s => s.Nome);
    }

    // ──────────────────────────────────────────
    // JOGADORES
    // ──────────────────────────────────────────

    private static void SeedJogadores(AppDbContext context, Dictionary<string, Selecao> sel)
    {
        var jogadores = new List<Jogador>
        {
            // Argentina
            J(sel["Argentina"],    "Lionel Messi",        "Atacante",  new DateOnly(1987, 6, 24),  13, 5),
            J(sel["Argentina"],    "Ángel Di María",      "Atacante",  new DateOnly(1988, 2, 14),   6, 5),
            J(sel["Argentina"],    "Rodrigo De Paul",     "Meio-campo", new DateOnly(1994, 5, 24),  2, 3),
            J(sel["Argentina"],    "Emiliano Martínez",   "Goleiro",   new DateOnly(1992, 9, 2),    0, 3),

            // Brasil
            J(sel["Brasil"],       "Vinícius Júnior",     "Atacante",  new DateOnly(2000, 7, 12),   3, 2),
            J(sel["Brasil"],       "Rodrygo",             "Atacante",  new DateOnly(2001, 1, 9),    0, 1),
            J(sel["Brasil"],       "Lucas Paquetá",       "Meio-campo", new DateOnly(1997, 8, 27),  5, 2),
            J(sel["Brasil"],       "Alisson Becker",      "Goleiro",   new DateOnly(1992, 10, 2),   0, 2),

            // França
            J(sel["França"],       "Kylian Mbappé",       "Atacante",  new DateOnly(1998, 12, 20), 12, 3),
            J(sel["França"],       "Antoine Griezmann",   "Atacante",  new DateOnly(1991, 3, 21),  11, 4),
            J(sel["França"],       "Aurélien Tchouaméni", "Meio-campo", new DateOnly(2000, 1, 27),  1, 2),
            J(sel["França"],       "Mike Maignan",        "Goleiro",   new DateOnly(1995, 7, 3),    0, 1),

            // Alemanha
            J(sel["Alemanha"],     "Jamal Musiala",       "Atacante",  new DateOnly(2003, 2, 26),   5, 2),
            J(sel["Alemanha"],     "Florian Wirtz",       "Meio-campo", new DateOnly(2003, 5, 3),   3, 2),
            J(sel["Alemanha"],     "Joshua Kimmich",      "Meio-campo", new DateOnly(1995, 2, 8),   9, 3),
            J(sel["Alemanha"],     "Manuel Neuer",        "Goleiro",   new DateOnly(1986, 3, 27),   0, 4),

            // Espanha
            J(sel["Espanha"],      "Lamine Yamal",        "Atacante",  new DateOnly(2007, 7, 13),   4, 2),
            J(sel["Espanha"],      "Pedri",               "Meio-campo", new DateOnly(2002, 11, 25), 7, 2),
            J(sel["Espanha"],      "Rodri",               "Meio-campo", new DateOnly(1996, 6, 22),  5, 3),
            J(sel["Espanha"],      "David Raya",          "Goleiro",   new DateOnly(1995, 9, 15),   0, 1),

            // Inglaterra
            J(sel["Inglaterra"],   "Jude Bellingham",     "Meio-campo", new DateOnly(2003, 6, 29),  10, 2),
            J(sel["Inglaterra"],   "Harry Kane",          "Atacante",  new DateOnly(1993, 7, 28),   68, 4),
            J(sel["Inglaterra"],   "Bukayo Saka",         "Atacante",  new DateOnly(2001, 9, 5),    15, 2),
            J(sel["Inglaterra"],   "Jordan Pickford",     "Goleiro",   new DateOnly(1994, 3, 7),     0, 3),

            // Portugal
            J(sel["Portugal"],     "Cristiano Ronaldo",   "Atacante",  new DateOnly(1985, 2, 5),   14, 6),
            J(sel["Portugal"],     "Bernardo Silva",      "Meio-campo", new DateOnly(1994, 8, 10),  8, 4),
            J(sel["Portugal"],     "Rafael Leão",         "Atacante",  new DateOnly(1999, 6, 10),   4, 2),
            J(sel["Portugal"],     "Rúben Dias",          "Defensor",  new DateOnly(1997, 5, 14),   0, 3),

            // Holanda
            J(sel["Holanda"],      "Virgil van Dijk",     "Defensor",  new DateOnly(1991, 7, 8),    5, 3),
            J(sel["Holanda"],      "Cody Gakpo",          "Atacante",  new DateOnly(2000, 5, 7),   10, 2),
            J(sel["Holanda"],      "Frenkie de Jong",     "Meio-campo", new DateOnly(1997, 5, 12),  2, 3),

            // Bélgica
            J(sel["Bélgica"],      "Romelu Lukaku",       "Atacante",  new DateOnly(1993, 5, 13),  14, 4),
            J(sel["Bélgica"],      "Kevin De Bruyne",     "Meio-campo", new DateOnly(1991, 6, 28), 26, 4),
            J(sel["Bélgica"],      "Thibaut Courtois",    "Goleiro",   new DateOnly(1992, 5, 11),   0, 4),

            // Itália
            J(sel["Itália"],       "Gianluigi Donnarumma","Goleiro",   new DateOnly(1999, 2, 25),   0, 3),
            J(sel["Itália"],       "Federico Chiesa",     "Atacante",  new DateOnly(1997, 10, 25),  9, 2),
            J(sel["Itália"],       "Sandro Tonali",       "Meio-campo", new DateOnly(2000, 5, 8),   1, 2),

            // Croácia
            J(sel["Croácia"],      "Luka Modrić",         "Meio-campo", new DateOnly(1985, 9, 9),   23, 5),
            J(sel["Croácia"],      "Ivan Perišić",        "Atacante",  new DateOnly(1989, 2, 2),    33, 5),
            J(sel["Croácia"],      "Mateo Kovačić",       "Meio-campo", new DateOnly(1994, 5, 6),    5, 4),

            // Japão
            J(sel["Japão"],        "Takumi Minamino",     "Atacante",  new DateOnly(1995, 1, 16),  24, 3),
            J(sel["Japão"],        "Ritsu Doan",          "Atacante",  new DateOnly(1998, 6, 16),  14, 2),
            J(sel["Japão"],        "Wataru Endo",         "Meio-campo", new DateOnly(1993, 2, 9),   4, 3),

            // Colômbia
            J(sel["Colômbia"],     "James Rodríguez",     "Meio-campo", new DateOnly(1991, 7, 12), 28, 4),
            J(sel["Colômbia"],     "Luis Díaz",           "Atacante",  new DateOnly(1997, 1, 13),  11, 2),
            J(sel["Colômbia"],     "Jhon Córdoba",        "Atacante",  new DateOnly(1993, 11, 11), 12, 2),

            // Uruguai
            J(sel["Uruguai"],      "Federico Valverde",   "Meio-campo", new DateOnly(1998, 7, 22),  9, 2),
            J(sel["Uruguai"],      "Darwin Núñez",        "Atacante",  new DateOnly(2000, 6, 24),  12, 2),
            J(sel["Uruguai"],      "Luis Suárez",         "Atacante",  new DateOnly(1987, 1, 24),  68, 5),

            // Coreia do Sul
            J(sel["Coreia do Sul"],"Son Heung-min",       "Atacante",  new DateOnly(1992, 7, 8),   35, 4),
            J(sel["Coreia do Sul"],"Lee Kang-in",         "Meio-campo", new DateOnly(2001, 2, 19), 10, 2),
            J(sel["Coreia do Sul"],"Kim Min-jae",         "Defensor",  new DateOnly(1996, 11, 15),  2, 2),

            // Senegal
            J(sel["Senegal"],      "Sadio Mané",          "Atacante",  new DateOnly(1992, 4, 10),  36, 3),
            J(sel["Senegal"],      "Edouard Mendy",       "Goleiro",   new DateOnly(1992, 3, 1),    0, 2),
            J(sel["Senegal"],      "Ismaïla Sarr",        "Atacante",  new DateOnly(1998, 2, 25),  12, 2),

            // Marrocos
            J(sel["Marrocos"],     "Hakim Ziyech",        "Atacante",  new DateOnly(1993, 3, 19),   6, 3),
            J(sel["Marrocos"],     "Achraf Hakimi",       "Defensor",  new DateOnly(1998, 11, 4),   4, 3),
            J(sel["Marrocos"],     "Yassine Bounou",      "Goleiro",   new DateOnly(1991, 4, 5),    0, 3),

            // Suíça
            J(sel["Suíça"],        "Granit Xhaka",        "Meio-campo", new DateOnly(1992, 9, 27), 11, 4),
            J(sel["Suíça"],        "Xherdan Shaqiri",     "Atacante",  new DateOnly(1991, 10, 10), 13, 4),
            J(sel["Suíça"],        "Yann Sommer",         "Goleiro",   new DateOnly(1988, 12, 17),  0, 4),

            // Estados Unidos
            J(sel["Estados Unidos"],"Christian Pulisic",  "Atacante",  new DateOnly(1998, 9, 18),  30, 3),
            J(sel["Estados Unidos"],"Tyler Adams",        "Meio-campo", new DateOnly(1999, 2, 14),  4, 2),
            J(sel["Estados Unidos"],"Matt Turner",        "Goleiro",   new DateOnly(1994, 6, 24),   0, 2),

            // México
            J(sel["México"],       "Hirving Lozano",      "Atacante",  new DateOnly(1995, 7, 30),  30, 4),
            J(sel["México"],       "Edson Álvarez",       "Meio-campo", new DateOnly(1997, 10, 24), 9, 3),
            J(sel["México"],       "Guillermo Ochoa",     "Goleiro",   new DateOnly(1985, 7, 13),   0, 5),

            // Canadá
            J(sel["Canadá"],       "Alphonso Davies",     "Defensor",  new DateOnly(2000, 11, 2),  13, 2),
            J(sel["Canadá"],       "Jonathan David",      "Atacante",  new DateOnly(2000, 1, 14),  32, 2),
            J(sel["Canadá"],       "Tajon Buchanan",      "Atacante",  new DateOnly(1999, 2, 8),   10, 2),

            // Equador
            J(sel["Equador"],      "Enner Valencia",      "Atacante",  new DateOnly(1989, 11, 4),  40, 4),
            J(sel["Equador"],      "Moisés Caicedo",      "Meio-campo", new DateOnly(2001, 11, 2),  2, 2),
            J(sel["Equador"],      "Byron Castillo",      "Defensor",  new DateOnly(1998, 11, 28),  0, 2),

            // Sérvia
            J(sel["Sérvia"],       "Aleksandar Mitrović", "Atacante",  new DateOnly(1994, 9, 16),  58, 3),
            J(sel["Sérvia"],       "Dušan Vlahović",      "Atacante",  new DateOnly(2000, 1, 28),  21, 2),
            J(sel["Sérvia"],       "Dušan Tadić",         "Atacante",  new DateOnly(1988, 11, 20), 20, 4),

            // Polônia
            J(sel["Polônia"],      "Robert Lewandowski",  "Atacante",  new DateOnly(1988, 8, 21),  82, 5),
            J(sel["Polônia"],      "Piotr Zieliński",     "Meio-campo", new DateOnly(1994, 6, 20), 22, 4),
            J(sel["Polônia"],      "Wojciech Szczęsny",   "Goleiro",   new DateOnly(1990, 4, 18),   0, 4),

            // Turquia
            J(sel["Turquia"],      "Hakan Çalhanoğlu",    "Meio-campo", new DateOnly(1994, 2, 8),  19, 3),
            J(sel["Turquia"],      "Arda Güler",          "Meio-campo", new DateOnly(2005, 2, 25),  7, 2),
            J(sel["Turquia"],      "Cenk Tosun",          "Atacante",  new DateOnly(1991, 6, 7),   30, 3),

            // Áustria
            J(sel["Áustria"],      "Marcel Sabitzer",     "Meio-campo", new DateOnly(1994, 3, 17), 11, 3),
            J(sel["Áustria"],      "Marko Arnautovic",    "Atacante",  new DateOnly(1989, 4, 19),  37, 4),
            J(sel["Áustria"],      "Christoph Baumgartner","Meio-campo",new DateOnly(1999, 8, 1),   9, 2),

            // Dinamarca
            J(sel["Dinamarca"],    "Christian Eriksen",   "Meio-campo", new DateOnly(1992, 2, 14), 43, 4),
            J(sel["Dinamarca"],    "Rasmus Højlund",      "Atacante",  new DateOnly(2003, 2, 4),   15, 2),
            J(sel["Dinamarca"],    "Pierre-Emile Højbjerg","Meio-campo",new DateOnly(1995, 8, 5),   7, 3),

            // Escócia
            J(sel["Escócia"],      "Andy Robertson",      "Defensor",  new DateOnly(1994, 3, 11),   6, 3),
            J(sel["Escócia"],      "Scott McTominay",     "Meio-campo", new DateOnly(1996, 12, 8), 16, 3),
            J(sel["Escócia"],      "Lyndon Dykes",        "Atacante",  new DateOnly(1995, 10, 7),  14, 2),

            // Irã
            J(sel["Irã"],          "Sardar Azmoun",       "Atacante",  new DateOnly(1995, 1, 1),   48, 3),
            J(sel["Irã"],          "Ali Gholizadeh",      "Atacante",  new DateOnly(1996, 3, 7),   10, 2),
            J(sel["Irã"],          "Alireza Jahanbakhsh", "Atacante",  new DateOnly(1993, 8, 11),  37, 3),

            // Austrália
            J(sel["Austrália"],    "Mathew Ryan",         "Goleiro",   new DateOnly(1992, 4, 8),    0, 4),
            J(sel["Austrália"],    "Socceroos Jackson Irvine","Meio-campo",new DateOnly(1993, 3, 7),7, 3),
            J(sel["Austrália"],    "Mitchell Duke",       "Atacante",  new DateOnly(1991, 1, 18),   6, 2),

            // Nigéria
            J(sel["Nigéria"],      "Victor Osimhen",      "Atacante",  new DateOnly(1998, 12, 29), 35, 2),
            J(sel["Nigéria"],      "Alex Iwobi",          "Meio-campo", new DateOnly(1996, 5, 3),  12, 3),
            J(sel["Nigéria"],      "William Troost-Ekong","Defensor",  new DateOnly(1993, 9, 1),    4, 3),

            // Egito
            J(sel["Egito"],        "Mohamed Salah",       "Atacante",  new DateOnly(1992, 6, 15),  58, 4),
            J(sel["Egito"],        "Omar Marmoush",       "Atacante",  new DateOnly(1999, 2, 7),   13, 2),
            J(sel["Egito"],        "Ahmed El-Shenawy",    "Goleiro",   new DateOnly(1988, 1, 25),   0, 2),

            // Arábia Saudita
            J(sel["Arábia Saudita"],"Salem Al-Dawsari",   "Atacante",  new DateOnly(1991, 8, 19),  23, 3),
            J(sel["Arábia Saudita"],"Mohammed Al-Owais",  "Goleiro",   new DateOnly(1991, 8, 11),   0, 3),
            J(sel["Arábia Saudita"],"Saleh Al-Shehri",    "Atacante",  new DateOnly(1993, 11, 3),   8, 2),

            // Argélia
            J(sel["Argélia"],      "Riyad Mahrez",        "Atacante",  new DateOnly(1991, 2, 21),  36, 4),
            J(sel["Argélia"],      "Islam Slimani",       "Atacante",  new DateOnly(1988, 6, 18),  47, 4),
            J(sel["Argélia"],      "Said Benrahma",       "Atacante",  new DateOnly(1995, 8, 10),   9, 2),

            // Camarões
            J(sel["Camarões"],     "Vincent Aboubakar",   "Atacante",  new DateOnly(1992, 1, 22),  39, 4),
            J(sel["Camarões"],     "Karl Toko Ekambi",    "Atacante",  new DateOnly(1992, 9, 14),  27, 3),
            J(sel["Camarões"],     "André Onana",         "Goleiro",   new DateOnly(1996, 4, 2),    0, 2),

            // Costa do Marfim
            J(sel["Costa do Marfim"],"Sébastien Haller",  "Atacante",  new DateOnly(1994, 6, 22),  14, 2),
            J(sel["Costa do Marfim"],"Franck Kessié",     "Meio-campo", new DateOnly(1996, 12, 19),10, 3),
            J(sel["Costa do Marfim"],"Simon Deli",        "Defensor",  new DateOnly(1991, 10, 13),  0, 2),

            // Venezuela
            J(sel["Venezuela"],    "Jhonder Cádiz",       "Atacante",  new DateOnly(1996, 2, 2),   10, 2),
            J(sel["Venezuela"],    "Tomás Rincón",        "Meio-campo", new DateOnly(1988, 1, 16),  4, 4),
            J(sel["Venezuela"],    "Ronald Hernández",    "Defensor",  new DateOnly(1997, 5, 8),    0, 2),

            // Paraguai
            J(sel["Paraguai"],     "Ángel Romero",        "Atacante",  new DateOnly(1992, 8, 13),  20, 3),
            J(sel["Paraguai"],     "Miguel Almirón",      "Meio-campo", new DateOnly(1994, 2, 10), 10, 3),
            J(sel["Paraguai"],     "Gustavo Gómez",       "Defensor",  new DateOnly(1993, 5, 6),    2, 2),

            // Costa Rica
            J(sel["Costa Rica"],   "Bryan Ruiz",          "Meio-campo", new DateOnly(1985, 8, 18), 24, 4),
            J(sel["Costa Rica"],   "Keylor Navas",        "Goleiro",   new DateOnly(1986, 12, 15),  0, 5),
            J(sel["Costa Rica"],   "Joel Campbell",       "Atacante",  new DateOnly(1992, 6, 26),  20, 4),

            // Honduras
            J(sel["Honduras"],     "Alberth Elis",        "Atacante",  new DateOnly(1996, 2, 12),  11, 2),
            J(sel["Honduras"],     "Romell Quioto",       "Atacante",  new DateOnly(1992, 8, 9),    5, 2),
            J(sel["Honduras"],     "Luis Palma",          "Atacante",  new DateOnly(2001, 1, 26),   3, 1),

            // Panamá
            J(sel["Panamá"],       "Rodolfo Pitti",       "Atacante",  new DateOnly(1991, 3, 30),  12, 2),
            J(sel["Panamá"],       "Alberto Quintero",    "Atacante",  new DateOnly(1989, 6, 10),   5, 2),
            J(sel["Panamá"],       "Anibal Godoy",        "Meio-campo", new DateOnly(1990, 3, 16),  1, 2),

            // Catar
            J(sel["Catar"],        "Akram Afif",          "Atacante",  new DateOnly(1996, 11, 18), 28, 2),
            J(sel["Catar"],        "Almoez Ali",          "Atacante",  new DateOnly(1996, 8, 19),  42, 2),
            J(sel["Catar"],        "Hassan Al-Haydos",    "Meio-campo", new DateOnly(1990, 12, 11),71, 3),

            // Iraque
            J(sel["Iraque"],       "Aymen Hussein",       "Atacante",  new DateOnly(1994, 4, 15),  22, 2),
            J(sel["Iraque"],       "Ali Jasim",           "Atacante",  new DateOnly(2001, 7, 25),   8, 1),
            J(sel["Iraque"],       "Amjad Attwan",        "Defensor",  new DateOnly(1993, 8, 6),    0, 1),

            // Uzbequistão
            J(sel["Uzbequistão"],  "Eldor Shomurodov",    "Atacante",  new DateOnly(1995, 6, 29),  21, 2),
            J(sel["Uzbequistão"],  "Jaloliddin Masharipov","Atacante", new DateOnly(1993, 4, 14),  12, 2),
            J(sel["Uzbequistão"],  "Otabek Shukurov",     "Defensor",  new DateOnly(1992, 9, 8),    0, 2),

            // Jordânia
            J(sel["Jordânia"],     "Yazan Al-Naimat",     "Atacante",  new DateOnly(1999, 6, 15),   3, 1),
            J(sel["Jordânia"],     "Ahmad Hayel",         "Meio-campo", new DateOnly(1997, 3, 12),  2, 1),
            J(sel["Jordânia"],     "Baha' Abdel-Rahman",  "Goleiro",   new DateOnly(1989, 5, 20),   0, 1),

            // Nova Zelândia
            J(sel["Nova Zelândia"],"Chris Wood",          "Atacante",  new DateOnly(1991, 12, 7),  32, 3),
            J(sel["Nova Zelândia"],"Clayton Lewis",       "Meio-campo", new DateOnly(1997, 10, 5),  2, 2),
            J(sel["Nova Zelândia"],"Stefan Marinovic",    "Goleiro",   new DateOnly(1991, 1, 30),   0, 2),

            // África do Sul
            J(sel["África do Sul"],"Percy Tau",           "Atacante",  new DateOnly(1994, 5, 13),  15, 3),
            J(sel["África do Sul"],"Themba Zwane",        "Atacante",  new DateOnly(1992, 9, 28),  14, 2),
            J(sel["África do Sul"],"Ronwen Williams",     "Goleiro",   new DateOnly(1992, 3, 25),   0, 2),

            // Tunísia
            J(sel["Tunísia"],      "Youssef Msakni",      "Atacante",  new DateOnly(1990, 10, 28), 18, 3),
            J(sel["Tunísia"],      "Wahbi Khazri",        "Atacante",  new DateOnly(1991, 2, 8),   25, 4),
            J(sel["Tunísia"],      "Ali Maâloul",         "Defensor",  new DateOnly(1990, 1, 1),    5, 4),
        };

        context.Jogadores.AddRange(jogadores);
    }

    private static Jogador J(Selecao s, string nome, string posicao, DateOnly nasc, int gols, int copas) =>
        new() { Selecao = s, Nome = nome, Posicao = posicao, DataNascimento = nasc, GolsMarcados = gols, ParticipacoesCopa = copas };

    // ──────────────────────────────────────────
    // JOGOS
    // ──────────────────────────────────────────

    private static void SeedJogos(AppDbContext context, Dictionary<string, Selecao> sel, Dictionary<string, Grupo> grupos)
    {
        // Copa 2026: 12 grupos × 6 jogos = 72 jogos na fase de grupos
        // Datas aproximadas: 12 jun a 30 jun 2026
        // Estádios Copa 2026 (16 sedes: 11 EUA, 2 Canadá, 3 México)

        var jogos = new List<Jogo>
        {
            // ── Grupo A: EUA, Inglaterra, Venezuela, Camarões ──
            Jogo(sel, grupos, "Estados Unidos",  "Venezuela",    "A", new DateTime(2026, 6, 12, 21, 0, 0), "SoFi Stadium",          "Los Angeles"),
            Jogo(sel, grupos, "Inglaterra",      "Camarões",     "A", new DateTime(2026, 6, 12, 18, 0, 0), "MetLife Stadium",        "East Rutherford"),
            Jogo(sel, grupos, "Estados Unidos",  "Inglaterra",   "A", new DateTime(2026, 6, 17, 21, 0, 0), "SoFi Stadium",          "Los Angeles"),
            Jogo(sel, grupos, "Venezuela",       "Camarões",     "A", new DateTime(2026, 6, 17, 18, 0, 0), "Rose Bowl",              "Pasadena"),
            Jogo(sel, grupos, "Estados Unidos",  "Camarões",     "A", new DateTime(2026, 6, 22, 21, 0, 0), "Levi's Stadium",         "Santa Clara"),
            Jogo(sel, grupos, "Inglaterra",      "Venezuela",    "A", new DateTime(2026, 6, 22, 21, 0, 0), "MetLife Stadium",        "East Rutherford"),

            // ── Grupo B: México, Polônia, Paraguai, Tunísia ──
            Jogo(sel, grupos, "México",          "Polônia",      "B", new DateTime(2026, 6, 13, 21, 0, 0), "Estadio Azteca",         "Cidade do México"),
            Jogo(sel, grupos, "Paraguai",        "Tunísia",      "B", new DateTime(2026, 6, 13, 18, 0, 0), "Estadio BBVA",           "Monterrey"),
            Jogo(sel, grupos, "México",          "Tunísia",      "B", new DateTime(2026, 6, 18, 21, 0, 0), "Estadio Azteca",         "Cidade do México"),
            Jogo(sel, grupos, "Polônia",         "Paraguai",     "B", new DateTime(2026, 6, 18, 18, 0, 0), "Estadio Akron",          "Guadalajara"),
            Jogo(sel, grupos, "México",          "Paraguai",     "B", new DateTime(2026, 6, 23, 21, 0, 0), "Estadio BBVA",           "Monterrey"),
            Jogo(sel, grupos, "Polônia",         "Tunísia",      "B", new DateTime(2026, 6, 23, 21, 0, 0), "Estadio Azteca",         "Cidade do México"),

            // ── Grupo C: Canadá, Croácia, Equador, Marrocos ──
            Jogo(sel, grupos, "Canadá",          "Marrocos",     "C", new DateTime(2026, 6, 13, 21, 0, 0), "BC Place",               "Vancouver"),
            Jogo(sel, grupos, "Croácia",         "Equador",      "C", new DateTime(2026, 6, 13, 18, 0, 0), "BMO Field",              "Toronto"),
            Jogo(sel, grupos, "Canadá",          "Croácia",      "C", new DateTime(2026, 6, 18, 21, 0, 0), "BMO Field",              "Toronto"),
            Jogo(sel, grupos, "Equador",         "Marrocos",     "C", new DateTime(2026, 6, 18, 18, 0, 0), "BC Place",               "Vancouver"),
            Jogo(sel, grupos, "Canadá",          "Equador",      "C", new DateTime(2026, 6, 23, 21, 0, 0), "BC Place",               "Vancouver"),
            Jogo(sel, grupos, "Croácia",         "Marrocos",     "C", new DateTime(2026, 6, 23, 21, 0, 0), "BMO Field",              "Toronto"),

            // ── Grupo D: França, Sérvia, Argentina, Japão ──
            Jogo(sel, grupos, "França",          "Argentina",    "D", new DateTime(2026, 6, 14, 21, 0, 0), "AT&T Stadium",           "Arlington"),
            Jogo(sel, grupos, "Sérvia",          "Japão",        "D", new DateTime(2026, 6, 14, 18, 0, 0), "Lumen Field",            "Seattle"),
            Jogo(sel, grupos, "França",          "Japão",        "D", new DateTime(2026, 6, 19, 21, 0, 0), "AT&T Stadium",           "Arlington"),
            Jogo(sel, grupos, "Argentina",       "Sérvia",       "D", new DateTime(2026, 6, 19, 18, 0, 0), "MetLife Stadium",        "East Rutherford"),
            Jogo(sel, grupos, "França",          "Sérvia",       "D", new DateTime(2026, 6, 24, 21, 0, 0), "Lumen Field",            "Seattle"),
            Jogo(sel, grupos, "Argentina",       "Japão",        "D", new DateTime(2026, 6, 24, 21, 0, 0), "AT&T Stadium",           "Arlington"),

            // ── Grupo E: Espanha, Turquia, Colômbia, África do Sul ──
            Jogo(sel, grupos, "Espanha",         "Colômbia",     "E", new DateTime(2026, 6, 14, 21, 0, 0), "Hard Rock Stadium",      "Miami"),
            Jogo(sel, grupos, "Turquia",         "África do Sul","E", new DateTime(2026, 6, 14, 18, 0, 0), "Gillette Stadium",       "Boston"),
            Jogo(sel, grupos, "Espanha",         "Turquia",      "E", new DateTime(2026, 6, 19, 21, 0, 0), "Commanders Field",       "Washington D.C."),
            Jogo(sel, grupos, "Colômbia",        "África do Sul","E", new DateTime(2026, 6, 19, 18, 0, 0), "Hard Rock Stadium",      "Miami"),
            Jogo(sel, grupos, "Espanha",         "África do Sul","E", new DateTime(2026, 6, 24, 21, 0, 0), "Gillette Stadium",       "Boston"),
            Jogo(sel, grupos, "Colômbia",        "Turquia",      "E", new DateTime(2026, 6, 24, 21, 0, 0), "Hard Rock Stadium",      "Miami"),

            // ── Grupo F: Alemanha, Suíça, Brasil, Argélia ──
            Jogo(sel, grupos, "Alemanha",        "Suíça",        "F", new DateTime(2026, 6, 15, 21, 0, 0), "Lincoln Financial Field","Philadelphia"),
            Jogo(sel, grupos, "Brasil",          "Argélia",      "F", new DateTime(2026, 6, 15, 18, 0, 0), "Arrowhead Stadium",      "Kansas City"),
            Jogo(sel, grupos, "Alemanha",        "Brasil",       "F", new DateTime(2026, 6, 20, 21, 0, 0), "MetLife Stadium",        "East Rutherford"),
            Jogo(sel, grupos, "Suíça",           "Argélia",      "F", new DateTime(2026, 6, 20, 18, 0, 0), "Lincoln Financial Field","Philadelphia"),
            Jogo(sel, grupos, "Alemanha",        "Argélia",      "F", new DateTime(2026, 6, 25, 21, 0, 0), "Arrowhead Stadium",      "Kansas City"),
            Jogo(sel, grupos, "Brasil",          "Suíça",        "F", new DateTime(2026, 6, 25, 21, 0, 0), "Lincoln Financial Field","Philadelphia"),

            // ── Grupo G: Portugal, Escócia, Uruguai, Coreia do Sul ──
            Jogo(sel, grupos, "Portugal",        "Uruguai",      "G", new DateTime(2026, 6, 15, 21, 0, 0), "Rose Bowl",              "Pasadena"),
            Jogo(sel, grupos, "Escócia",         "Coreia do Sul","G", new DateTime(2026, 6, 15, 18, 0, 0), "SoFi Stadium",           "Los Angeles"),
            Jogo(sel, grupos, "Portugal",        "Escócia",      "G", new DateTime(2026, 6, 20, 21, 0, 0), "Rose Bowl",              "Pasadena"),
            Jogo(sel, grupos, "Uruguai",         "Coreia do Sul","G", new DateTime(2026, 6, 20, 18, 0, 0), "Levi's Stadium",         "Santa Clara"),
            Jogo(sel, grupos, "Portugal",        "Coreia do Sul","G", new DateTime(2026, 6, 25, 21, 0, 0), "Rose Bowl",              "Pasadena"),
            Jogo(sel, grupos, "Uruguai",         "Escócia",      "G", new DateTime(2026, 6, 25, 21, 0, 0), "Levi's Stadium",         "Santa Clara"),

            // ── Grupo H: Holanda, Irã, Senegal, Honduras ──
            Jogo(sel, grupos, "Holanda",         "Senegal",      "H", new DateTime(2026, 6, 16, 21, 0, 0), "Commanders Field",       "Washington D.C."),
            Jogo(sel, grupos, "Irã",             "Honduras",     "H", new DateTime(2026, 6, 16, 18, 0, 0), "AT&T Stadium",           "Arlington"),
            Jogo(sel, grupos, "Holanda",         "Irã",          "H", new DateTime(2026, 6, 21, 21, 0, 0), "MetLife Stadium",        "East Rutherford"),
            Jogo(sel, grupos, "Senegal",         "Honduras",     "H", new DateTime(2026, 6, 21, 18, 0, 0), "Hard Rock Stadium",      "Miami"),
            Jogo(sel, grupos, "Holanda",         "Honduras",     "H", new DateTime(2026, 6, 26, 21, 0, 0), "Commanders Field",       "Washington D.C."),
            Jogo(sel, grupos, "Irã",             "Senegal",      "H", new DateTime(2026, 6, 26, 21, 0, 0), "AT&T Stadium",           "Arlington"),

            // ── Grupo I: Bélgica, Austrália, Egito, Costa Rica ──
            Jogo(sel, grupos, "Bélgica",         "Austrália",    "I", new DateTime(2026, 6, 16, 21, 0, 0), "Lumen Field",            "Seattle"),
            Jogo(sel, grupos, "Egito",           "Costa Rica",   "I", new DateTime(2026, 6, 16, 18, 0, 0), "Arrowhead Stadium",      "Kansas City"),
            Jogo(sel, grupos, "Bélgica",         "Egito",        "I", new DateTime(2026, 6, 21, 21, 0, 0), "Gillette Stadium",       "Boston"),
            Jogo(sel, grupos, "Austrália",       "Costa Rica",   "I", new DateTime(2026, 6, 21, 18, 0, 0), "Lumen Field",            "Seattle"),
            Jogo(sel, grupos, "Bélgica",         "Costa Rica",   "I", new DateTime(2026, 6, 26, 21, 0, 0), "Gillette Stadium",       "Boston"),
            Jogo(sel, grupos, "Austrália",       "Egito",        "I", new DateTime(2026, 6, 26, 21, 0, 0), "Arrowhead Stadium",      "Kansas City"),

            // ── Grupo J: Itália, Arábia Saudita, Nigéria, Panamá ──
            Jogo(sel, grupos, "Itália",          "Nigéria",      "J", new DateTime(2026, 6, 17, 21, 0, 0), "Lincoln Financial Field","Philadelphia"),
            Jogo(sel, grupos, "Arábia Saudita",  "Panamá",       "J", new DateTime(2026, 6, 17, 18, 0, 0), "SoFi Stadium",           "Los Angeles"),
            Jogo(sel, grupos, "Itália",          "Arábia Saudita","J", new DateTime(2026, 6, 22, 21, 0, 0), "Rose Bowl",              "Pasadena"),
            Jogo(sel, grupos, "Nigéria",         "Panamá",       "J", new DateTime(2026, 6, 22, 18, 0, 0), "Hard Rock Stadium",      "Miami"),
            Jogo(sel, grupos, "Itália",          "Panamá",       "J", new DateTime(2026, 6, 27, 21, 0, 0), "Lincoln Financial Field","Philadelphia"),
            Jogo(sel, grupos, "Nigéria",         "Arábia Saudita","J", new DateTime(2026, 6, 27, 21, 0, 0), "SoFi Stadium",           "Los Angeles"),

            // ── Grupo K: Áustria, Catar, Costa do Marfim, Nova Zelândia ──
            Jogo(sel, grupos, "Áustria",         "Costa do Marfim","K",new DateTime(2026, 6, 17, 21, 0, 0), "Levi's Stadium",         "Santa Clara"),
            Jogo(sel, grupos, "Catar",           "Nova Zelândia", "K", new DateTime(2026, 6, 17, 18, 0, 0), "Estadio Akron",          "Guadalajara"),
            Jogo(sel, grupos, "Áustria",         "Catar",         "K", new DateTime(2026, 6, 22, 21, 0, 0), "Estadio Azteca",         "Cidade do México"),
            Jogo(sel, grupos, "Costa do Marfim", "Nova Zelândia", "K", new DateTime(2026, 6, 22, 18, 0, 0), "Estadio BBVA",           "Monterrey"),
            Jogo(sel, grupos, "Áustria",         "Nova Zelândia", "K", new DateTime(2026, 6, 27, 21, 0, 0), "Estadio Akron",          "Guadalajara"),
            Jogo(sel, grupos, "Catar",           "Costa do Marfim","K",new DateTime(2026, 6, 27, 21, 0, 0), "Estadio Azteca",         "Cidade do México"),

            // ── Grupo L: Dinamarca, Iraque, Uzbequistão, Jordânia ──
            Jogo(sel, grupos, "Dinamarca",       "Uzbequistão",  "L", new DateTime(2026, 6, 18, 21, 0, 0), "BC Place",               "Vancouver"),
            Jogo(sel, grupos, "Iraque",          "Jordânia",     "L", new DateTime(2026, 6, 18, 18, 0, 0), "BMO Field",              "Toronto"),
            Jogo(sel, grupos, "Dinamarca",       "Iraque",       "L", new DateTime(2026, 6, 23, 21, 0, 0), "BC Place",               "Vancouver"),
            Jogo(sel, grupos, "Uzbequistão",     "Jordânia",     "L", new DateTime(2026, 6, 23, 18, 0, 0), "BMO Field",              "Toronto"),
            Jogo(sel, grupos, "Dinamarca",       "Jordânia",     "L", new DateTime(2026, 6, 28, 21, 0, 0), "BC Place",               "Vancouver"),
            Jogo(sel, grupos, "Uzbequistão",     "Iraque",       "L", new DateTime(2026, 6, 28, 21, 0, 0), "BMO Field",              "Toronto"),
        };

        context.Jogos.AddRange(jogos);
    }

    private static Jogo Jogo(
        Dictionary<string, Selecao> sel,
        Dictionary<string, Grupo> grupos,
        string selA, string selB,
        string grupo, DateTime dataHora, string estadio, string cidade) => new()
    {
        SelecaoA = sel[selA],
        SelecaoB = sel[selB],
        Grupo = grupos[grupo],
        DataHora = dataHora,
        Estadio = estadio,
        Cidade = cidade,
        Fase = "Grupo"
    };

    // ──────────────────────────────────────────
    // RANKING FIFA
    // ──────────────────────────────────────────

    private static void SeedRankingFifa(AppDbContext context, Dictionary<string, Selecao> sel)
    {
        var dataAtualizacao = new DateTime(2026, 4, 4);

        var rankings = sel.Values.Select(s => new RankingFifa
        {
            Selecao = s,
            Posicao = s.PosicaoRanking,
            Pontuacao = s.PontuacaoRanking,
            DataAtualizacao = dataAtualizacao
        }).ToList();

        context.RankingFifa.AddRange(rankings);
    }
}
