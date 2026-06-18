## ADDED Requirements

### Requirement: SeedData popula banco na primeira inicialização
O sistema SHALL verificar se o banco já contém dados antes de inserir e SHALL popular automaticamente na primeira inicialização via `IHostedService`.

#### Scenario: Banco vazio recebe dados no startup
- **WHEN** a aplicação inicia com banco SQLite vazio
- **THEN** grupos, seleções, jogadores, jogos e ranking FIFA são inseridos automaticamente

#### Scenario: Banco já populado não recebe duplicatas
- **WHEN** a aplicação é reiniciada com banco já populado
- **THEN** nenhum dado duplicado é inserido

### Requirement: Dados dos 12 grupos da Copa 2026
O sistema SHALL inserir todos os 12 grupos (A a L) da Copa do Mundo 2026 com suas respectivas seleções.

#### Scenario: 12 grupos criados
- **WHEN** o seed é executado
- **THEN** existem exatamente 12 grupos no banco (letras A a L)

#### Scenario: 4 seleções por grupo
- **WHEN** o seed é executado
- **THEN** cada grupo contém exatamente 4 seleções (total de 48 seleções)

### Requirement: Dados das 48 seleções participantes
O sistema SHALL inserir as 48 seleções qualificadas para a Copa do Mundo 2026 com nome, código FIFA (ISO 3166-1 alpha-2), grupo, posição e pontuação no ranking FIFA.

#### Scenario: Brasil presente com dados corretos
- **WHEN** o seed é executado
- **THEN** existe uma Selecao com Nome "Brasil" e CodigoFifa "BR"

#### Scenario: Todas seleções têm CodigoFifa preenchido
- **WHEN** o seed é executado
- **THEN** todas as 48 seleções possuem CodigoFifa não vazio

### Requirement: Dados dos jogadores por seleção
O sistema SHALL inserir ao menos 3 jogadores representativos por seleção, com nome, posição, data de nascimento, gols marcados em Copas e número de participações em Copas.

#### Scenario: Mínimo de jogadores por seleção
- **WHEN** o seed é executado
- **THEN** cada seleção possui ao menos 3 jogadores no banco

### Requirement: Dados dos jogos da fase de grupos
O sistema SHALL inserir todos os jogos da fase de grupos da Copa 2026 com data/hora, seleções, grupo, estádio e cidade.

#### Scenario: Jogos da fase de grupos criados
- **WHEN** o seed é executado
- **THEN** existem jogos com Fase = "Grupo" e GrupoId preenchido para cada combinação de seleções nos grupos

#### Scenario: Jogos sem resultado inicial
- **WHEN** o seed insere jogos da fase de grupos
- **THEN** GolsSelecaoA e GolsSelecaoB são null (Copa ainda em andamento ou não iniciada)

### Requirement: Dados do ranking FIFA inicial
O sistema SHALL inserir o ranking FIFA das 48 seleções participantes com posição, pontuação e data da última atualização.

#### Scenario: Ranking FIFA inserido para todas as seleções
- **WHEN** o seed é executado
- **THEN** cada uma das 48 seleções possui um registro em RankingFifa
