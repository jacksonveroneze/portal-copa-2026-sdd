## Context

O PortalCopa26 parte do zero: não existe ainda nenhum projeto .NET criado. O protótipo HTML validado (em `D:\Copa2026\prototipo`) serve como referência visual, mas toda a infraestrutura técnica precisa ser criada. O CLAUDE.md define as tecnologias (.NET 10, Blazor Web App, EF Core, SQLite, Bootstrap 5, Chart.js via JSInterop) e a estrutura de pastas desejada em `src/PortalCopa26/`.

A aplicação é um portal informativo da Copa do Mundo 2026, com consulta de jogos, grupos, seleções, ranking FIFA e simulação de resultados. Nesta fase, o foco é exclusivamente a fundação: não há UI, apenas infraestrutura e dados.

## Goals / Non-Goals

**Goals:**
- Criar solução `PortalCopa26.slnx` com projeto Blazor Web App (.NET 10) em `src/PortalCopa26/`
- Configurar EF Core 10 com provider SQLite
- Definir entidades do domínio com relacionamentos corretos
- Criar `AppDbContext` com DbSets e configurações Fluent API mínimas
- Popular banco de dados com dados reais da Copa 2026 via `IHostedService` de seed
- Configurar injeção de dependência (DI) para DbContext e serviços futuros
- Garantir estrutura de pastas que permita migração para camadas sem refatoração pesada

**Non-Goals:**
- Implementação de páginas ou componentes Blazor
- Integração com APIs externas
- Autenticação, autorização ou área administrativa
- Lógica de simulação (apenas as entidades de suporte)
- Migrations automáticas em produção (EF Core `EnsureCreated` é suficiente para SQLite local)

## Decisions

### D1: Blazor Web App com render mode Auto
**Decisão**: Usar Blazor Web App com `InteractiveAuto` disponível globalmente, mas páginas de conteúdo estático como SSR por padrão.
**Rationale**: Permite renderização server-side para SEO nas páginas informativas e interatividade WASM no simulador sem criar dois projetos. O CLAUDE.md especifica projeto único.
**Alternativa descartada**: Blazor Server puro — bloquearia funcionalidades offline no simulador futuro; Blazor WASM standalone — complicaria o acesso ao SQLite local.

### D2: EF Core com `EnsureCreated` em vez de Migrations
**Decisão**: Chamar `dbContext.Database.EnsureCreated()` na inicialização para criar o schema SQLite.
**Rationale**: Aplicação local sem múltiplos ambientes. SQLite é destruído/recriado facilmente durante desenvolvimento. Migrations adicionam complexidade sem benefício nesta fase.
**Alternativa descartada**: `dotnet ef migrations` — overhead desnecessário para SQLite local; pode ser adicionado na migração para arquitetura em camadas futura.

### D3: Seed via `IHostedService`
**Decisão**: Implementar `DatabaseSeeder` como `IHostedService` que verifica se o banco já foi populado antes de inserir dados.
**Rationale**: Executa no startup sem bloquear a requisição. A verificação por `dbContext.Selecoes.AnyAsync()` evita duplicação de dados em reinicializações.
**Alternativa descartada**: `HasData()` no OnModelCreating — não permite lógica condicional; `ModelBuilder.HasData` exige IDs fixos e complica dados relacionais complexos.

### D4: Estrutura de pastas preparada para camadas
**Decisão**: Criar pastas `Models/`, `Services/`, `Data/`, `Pages/`, `Components/` no projeto único. Interfaces de serviço em `Services/Interfaces/`.
**Rationale**: O CLAUDE.md exige que o código permita futura migração para arquitetura em camadas. Separar interfaces de implementações desde o início evita refatoração futura.

### D5: Dados das seleções — bandeiras via emoji/CDN público
**Decisão**: Armazenar código ISO 3166-1 alpha-2 na entidade `Selecao` e usar URL de API pública da FIFA ou CDN de emojis para renderizar bandeiras.
**Rationale**: O CLAUDE.md menciona explicitamente "bandeiras das seleções poderão utilizar o padrão da API pública da FIFA". Armazenar apenas o código garante flexibilidade.

### D6: Entidade `SimulacaoJogo` com scores nullable
**Decisão**: `GolsSelecaoA` e `GolsSelecaoB` como `int?` em `SimulacaoJogo`.
**Rationale**: Um jogo simulado pode ser criado sem resultado ainda (fase de planejamento da simulação). `null` representa "não simulado ainda".

## Risks / Trade-offs

- **[Risk] `EnsureCreated` não suporta schema evolution** → Mitigation: Ao migrar para arquitetura em camadas, substituir por Migrations do EF Core. Documentado no D2.
- **[Risk] Seed com ~48 seleções, ~736 jogadores e ~80 jogos pode ser lento no primeiro start** → Mitigation: Seed roda em background via `IHostedService`; app responde requisições enquanto popula. Adicionar log de progresso.
- **[Risk] Dados da Copa 2026 podem mudar até o torneio** → Mitigation: SeedData é código C# facilmente editável. Nenhum dado externo é consumido em runtime.
- **[Risk] Blazor InteractiveAuto requer configuração de circuito server + WASM** → Mitigation: Para esta fase, nenhuma interatividade é necessária. O render mode padrão SSR é suficiente até o simulador ser implementado.
