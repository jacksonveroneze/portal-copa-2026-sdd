using Microsoft.EntityFrameworkCore;
using PortalCopa26.Models;

namespace PortalCopa26.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<Selecao> Selecoes => Set<Selecao>();
    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<Jogo> Jogos => Set<Jogo>();
    public DbSet<RankingFifa> RankingFifa => Set<RankingFifa>();
    public DbSet<Simulacao> Simulacoes => Set<Simulacao>();
    public DbSet<SimulacaoJogo> SimulacaoJogos => Set<SimulacaoJogo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Jogo>(entity =>
        {
            entity.HasOne(j => j.SelecaoA)
                  .WithMany(s => s.JogosComoSelecaoA)
                  .HasForeignKey(j => j.SelecaoAId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(j => j.SelecaoB)
                  .WithMany(s => s.JogosComoSelecaoB)
                  .HasForeignKey(j => j.SelecaoBId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(j => j.Grupo)
                  .WithMany(g => g.Jogos)
                  .HasForeignKey(j => j.GrupoId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<RankingFifa>()
            .HasOne(r => r.Selecao)
            .WithOne(s => s.RankingFifa)
            .HasForeignKey<RankingFifa>(r => r.SelecaoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
