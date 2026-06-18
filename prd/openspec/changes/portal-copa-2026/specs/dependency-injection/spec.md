## ADDED Requirements

### Requirement: AppDbContext registrado no container DI
O sistema SHALL registrar `AppDbContext` com `AddDbContext<AppDbContext>` usando o provider SQLite e string de conexão configurada em `appsettings.json` ou como fallback inline.

#### Scenario: DbContext resolvido via DI
- **WHEN** um serviço solicita `AppDbContext` via injeção de dependência
- **THEN** o container entrega uma instância configurada com a connection string SQLite correta

#### Scenario: String de conexão em appsettings.json
- **WHEN** a aplicação inicia
- **THEN** a connection string do SQLite é lida de `ConnectionStrings:DefaultConnection` em `appsettings.json`

### Requirement: DatabaseSeeder registrado como IHostedService
O sistema SHALL registrar `DatabaseSeeder` como `IHostedService` para execução automática no startup da aplicação.

#### Scenario: Seed executa no startup sem intervenção manual
- **WHEN** `dotnet run` é executado
- **THEN** o banco é criado e populado automaticamente sem comandos adicionais

### Requirement: Serviços de dados preparados para futuras funcionalidades
O sistema SHALL definir interfaces de serviço em `Services/Interfaces/` para as principais entidades, com implementações básicas em `Services/`, mesmo que ainda sem uso nas páginas.

#### Scenario: Interface IJogoService existe
- **WHEN** o projeto é compilado
- **THEN** `IJogoService` está definida em `Services/Interfaces/IJogoService.cs`

#### Scenario: Interface ISelecaoService existe
- **WHEN** o projeto é compilado
- **THEN** `ISelecaoService` está definida em `Services/Interfaces/ISelecaoService.cs`

#### Scenario: Interface IGrupoService existe
- **WHEN** o projeto é compilado
- **THEN** `IGrupoService` está definida em `Services/Interfaces/IGrupoService.cs`

#### Scenario: Interface IRankingFifaService existe
- **WHEN** o projeto é compilado
- **THEN** `IRankingFifaService` está definida em `Services/Interfaces/IRankingFifaService.cs`

#### Scenario: Serviços registrados no container DI
- **WHEN** a aplicação inicia
- **THEN** todas as implementações de serviço estão registradas com seus respectivos ciclos de vida (Scoped)
