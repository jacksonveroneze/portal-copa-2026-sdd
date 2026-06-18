## 1. Criação da Solução e Projeto

- [x] 1.1 Criar a solução `PortalCopa26.slnx` em `src/PortalCopa26/` usando `dotnet new sln --format slnx`
- [x] 1.2 Criar o projeto Blazor Web App (.NET 10) `PortalCopa26` em `src/PortalCopa26/PortalCopa26/`
- [x] 1.3 Adicionar o projeto à solução com `dotnet sln add`
- [x] 1.4 Criar a estrutura de pastas: `Pages/`, `Components/`, `Models/`, `Services/`, `Services/Interfaces/`, `Data/`

## 2. Configuração de Dependências

- [x] 2.1 Adicionar pacote `Microsoft.EntityFrameworkCore.Sqlite` ao projeto
- [x] 2.2 Adicionar pacote `Microsoft.EntityFrameworkCore.Design` ao projeto
- [x] 2.3 Verificar que `dotnet build` conclui com sucesso após adicionar pacotes

## 3. Modelos do Domínio

- [x] 3.1 Criar `Models/Grupo.cs` com propriedades `Id`, `Nome`, `Letra` e coleção de `Selecao`
- [x] 3.2 Criar `Models/Selecao.cs` com propriedades `Id`, `Nome`, `CodigoFifa`, `GrupoId`, `PontuacaoRanking`, `PosicaoRanking` e navegações
- [x] 3.3 Criar `Models/Jogador.cs` com propriedades `Id`, `Nome`, `Posicao`, `DataNascimento`, `GolsMarcados`, `ParticipacoesCopa`, `SelecaoId`
- [x] 3.4 Criar `Models/Jogo.cs` com propriedades `Id`, `DataHora`, `SelecaoAId`, `SelecaoBId`, `GrupoId` (nullable), `Estadio`, `Cidade`, `Fase`, `GolsSelecaoA` (nullable), `GolsSelecaoB` (nullable)
- [x] 3.5 Criar `Models/RankingFifa.cs` com propriedades `Id`, `SelecaoId`, `Posicao`, `Pontuacao`, `DataAtualizacao`
- [x] 3.6 Criar `Models/Simulacao.cs` com propriedades `Id`, `DataCriacao`, `Nome` e coleção de `SimulacaoJogo`
- [x] 3.7 Criar `Models/SimulacaoJogo.cs` com propriedades `Id`, `SimulacaoId`, `JogoId`, `GolsSelecaoA` (nullable), `GolsSelecaoB` (nullable)

## 4. DbContext e Configuração EF Core

- [x] 4.1 Criar `Data/AppDbContext.cs` herdando de `DbContext` com DbSets para todas as entidades
- [x] 4.2 Configurar `OnModelCreating` com Fluent API: relacionamentos de `Jogo` com `SelecaoA` e `SelecaoB` usando `DeleteBehavior.Restrict`
- [x] 4.3 Configurar relacionamento `Jogo → Grupo` como nullable (fase de grupos opcional)
- [x] 4.4 Adicionar string de conexão SQLite em `appsettings.json` (`ConnectionStrings:DefaultConnection`)

## 5. Interfaces e Serviços

- [x] 5.1 Criar `Services/Interfaces/IGrupoService.cs` com assinaturas para listar grupos e obter grupo por id
- [x] 5.2 Criar `Services/Interfaces/ISelecaoService.cs` com assinaturas para listar seleções e obter seleção por id
- [x] 5.3 Criar `Services/Interfaces/IJogoService.cs` com assinaturas para listar jogos por grupo e por data
- [x] 5.4 Criar `Services/Interfaces/IRankingFifaService.cs` com assinatura para obter ranking ordenado por posição
- [x] 5.5 Criar implementações básicas `Services/GrupoService.cs`, `Services/SelecaoService.cs`, `Services/JogoService.cs`, `Services/RankingFifaService.cs` usando `AppDbContext`

## 6. Injeção de Dependência e Startup

- [x] 6.1 Registrar `AppDbContext` com `AddDbContext` usando SQLite e a connection string de `appsettings.json` em `Program.cs`
- [x] 6.2 Chamar `EnsureCreated()` ou usar migration no startup para criar o schema SQLite
- [x] 6.3 Registrar todos os serviços (`IGrupoService`, `ISelecaoService`, `IJogoService`, `IRankingFifaService`) como `Scoped` em `Program.cs`
- [x] 6.4 Registrar `DatabaseSeeder` como `IHostedService` em `Program.cs`

## 7. SeedData

- [x] 7.1 Criar `Data/DatabaseSeeder.cs` implementando `IHostedService` com verificação de banco já populado
- [x] 7.2 Implementar seed dos 12 grupos (A a L) da Copa do Mundo 2026
- [x] 7.3 Implementar seed das 48 seleções com nome, `CodigoFifa`, grupo, posição e pontuação no ranking FIFA
- [x] 7.4 Implementar seed de ao menos 3 jogadores representativos por seleção (nome, posição, data de nascimento, gols em Copas, participações)
- [x] 7.5 Implementar seed dos jogos da fase de grupos com data/hora, seleções, grupo, estádio e cidade
- [x] 7.6 Implementar seed dos registros `RankingFifa` para as 48 seleções com posição, pontuação e data de atualização

## 8. Validação Final

- [x] 8.1 Executar `dotnet build` e confirmar zero erros e zero warnings relevantes
- [x] 8.2 Executar `dotnet run` e confirmar que a aplicação inicia, o banco é criado e o seed é executado sem erros
- [x] 8.3 Confirmar que o arquivo `copa2026.db` (ou nome configurado) é criado na pasta correta
- [x] 8.4 Verificar via log ou consulta que grupos, seleções, jogadores, jogos e ranking foram inseridos corretamente
