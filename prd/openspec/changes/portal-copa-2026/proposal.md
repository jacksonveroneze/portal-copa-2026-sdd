## Why

O projeto PortalCopa26 não possui fundação técnica ainda: falta a solução .NET, o projeto Blazor, a configuração do EF Core/SQLite, as entidades do domínio e o seed de dados. Sem essa base, nenhuma funcionalidade de consulta ou simulação pode ser construída.

## What Changes

- Criar solução `PortalCopa26.slnx` com projeto Blazor Web App (.NET 10)
- Configurar EF Core com SQLite como banco de dados local
- Criar `AppDbContext` com mapeamento das entidades
- Criar as entidades do domínio: `Grupo`, `Selecao`, `Jogador`, `Jogo`, `RankingFifa`, `Simulacao`, `SimulacaoJogo`
- Implementar `SeedData` com dados reais da Copa do Mundo 2026 (grupos, seleções, jogadores, jogos, ranking FIFA)
- Configurar injeção de dependência (DbContext, serviços de dados)
- Definir estrutura de pastas (`Pages`, `Components`, `Models`, `Services`, `Data`) pronta para expansão futura

## Capabilities

### New Capabilities

- `project-foundation`: Solução .NET 10 com Blazor Web App configurado, incluindo EF Core, SQLite e estrutura de projeto
- `domain-model`: Entidades do domínio da Copa 2026 (Grupo, Selecao, Jogador, Jogo, RankingFifa, Simulacao, SimulacaoJogo) com relacionamentos e DbContext
- `seed-data`: Dados iniciais da Copa do Mundo 2026 carregados via SeedData (grupos, seleções, jogadores, jogos, ranking FIFA)
- `dependency-injection`: Configuração dos serviços e repositórios no container DI do ASP.NET Core

### Modified Capabilities

## Impact

- Cria a estrutura física `src/PortalCopa26/` conforme definido no CLAUDE.md
- Adiciona dependências NuGet: `Microsoft.EntityFrameworkCore.Sqlite`, `Microsoft.EntityFrameworkCore.Design`
- O banco SQLite (`copa2026.db`) será criado em tempo de execução na pasta da aplicação
- A estrutura de entidades e DbContext é a base para todas as funcionalidades futuras (Landing Page, Jogos, Grupos, Seleções, Ranking, Simulador)
- Nenhuma página ou componente visual é implementado nesta especificação
