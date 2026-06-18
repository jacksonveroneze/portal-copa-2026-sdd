## ADDED Requirements

### Requirement: Entidade Grupo
O sistema SHALL ter a entidade `Grupo` com propriedades `Id` (int), `Nome` (string), `Letra` (string) e coleção de navegação para `Selecao`.

#### Scenario: Grupo possui seleções associadas
- **WHEN** um Grupo é carregado com Include de Seleções
- **THEN** retorna a lista de Seleções pertencentes ao grupo

### Requirement: Entidade Selecao
O sistema SHALL ter a entidade `Selecao` com propriedades: `Id` (int), `Nome` (string), `CodigoFifa` (string — código ISO 3166-1 alpha-2), `GrupoId` (int FK), `PontuacaoRanking` (int), `PosicaoRanking` (int), e coleções de navegação para `Jogador`, `JogoComoSelecaoA`, `JogoComoSelecaoB`.

#### Scenario: Seleção referencia grupo
- **WHEN** uma Selecao é carregada com Include de Grupo
- **THEN** retorna o Grupo ao qual pertence

#### Scenario: Seleção possui código FIFA
- **WHEN** uma Selecao é criada com CodigoFifa "BR"
- **THEN** o campo CodigoFifa armazena "BR" para uso futuro em URLs de bandeiras

### Requirement: Entidade Jogador
O sistema SHALL ter a entidade `Jogador` com propriedades: `Id` (int), `Nome` (string), `Posicao` (string), `DataNascimento` (DateOnly), `GolsMarcados` (int), `ParticipacoesCopa` (int), `SelecaoId` (int FK).

#### Scenario: Jogador vinculado a seleção
- **WHEN** um Jogador é carregado com Include de Selecao
- **THEN** retorna a Seleção a qual o jogador pertence

#### Scenario: Idade calculável a partir de DataNascimento
- **WHEN** DataNascimento é armazenada
- **THEN** a idade do jogador pode ser calculada em runtime sem campo adicional

### Requirement: Entidade Jogo
O sistema SHALL ter a entidade `Jogo` com propriedades: `Id` (int), `DataHora` (DateTime), `SelecaoAId` (int FK), `SelecaoBId` (int FK), `GrupoId` (int FK nullable — null para fase eliminatória), `Estadio` (string), `Cidade` (string), `Fase` (string), `GolsSelecaoA` (int nullable), `GolsSelecaoB` (int nullable).

#### Scenario: Jogo da fase de grupos referencia grupo
- **WHEN** um Jogo com GrupoId preenchido é carregado
- **THEN** o Grupo associado está acessível via navegação

#### Scenario: Jogo sem resultado tem gols nulos
- **WHEN** um Jogo ainda não realizado é criado
- **THEN** GolsSelecaoA e GolsSelecaoB são null

### Requirement: Entidade RankingFifa
O sistema SHALL ter a entidade `RankingFifa` com propriedades: `Id` (int), `SelecaoId` (int FK), `Posicao` (int), `Pontuacao` (double), `DataAtualizacao` (DateTime).

#### Scenario: Ranking FIFA referencia seleção
- **WHEN** um RankingFifa é carregado com Include de Selecao
- **THEN** retorna a Seleção com os dados do ranking

### Requirement: Entidade Simulacao
O sistema SHALL ter a entidade `Simulacao` com propriedades: `Id` (int), `DataCriacao` (DateTime), `Nome` (string), e coleção de `SimulacaoJogo`.

#### Scenario: Simulação persiste após reinício da aplicação
- **WHEN** uma Simulacao é criada e a aplicação é reiniciada
- **THEN** a Simulacao permanece disponível no banco SQLite

### Requirement: Entidade SimulacaoJogo
O sistema SHALL ter a entidade `SimulacaoJogo` com propriedades: `Id` (int), `SimulacaoId` (int FK), `JogoId` (int FK), `GolsSelecaoA` (int nullable), `GolsSelecaoB` (int nullable).

#### Scenario: SimulacaoJogo sem resultado simulado
- **WHEN** um SimulacaoJogo é criado sem resultado
- **THEN** GolsSelecaoA e GolsSelecaoB são null

#### Scenario: SimulacaoJogo com resultado simulado
- **WHEN** GolsSelecaoA e GolsSelecaoB são definidos
- **THEN** os valores são persistidos no banco

### Requirement: AppDbContext configurado
O sistema SHALL ter um `AppDbContext` que herda de `DbContext` e expõe `DbSet<T>` para todas as entidades: `Grupos`, `Selecoes`, `Jogadores`, `Jogos`, `RankingFifa`, `Simulacoes`, `SimulacaoJogos`.

#### Scenario: AppDbContext resolve relacionamentos sem ciclos
- **WHEN** EF Core gera o schema
- **THEN** nenhum erro de relacionamento cíclico ou FK constraint é lançado

#### Scenario: Jogo usa DeleteBehavior.Restrict para seleções
- **WHEN** uma Selecao referenciada por Jogo é deletada
- **THEN** EF Core lança exceção em vez de deletar em cascata (protege integridade)
