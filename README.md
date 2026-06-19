# PortalCopa26

Portal informativo da Copa do Mundo 2026 focado em consulta de jogos, grupos, seleções, ranking FIFA e simulação de resultados.

## Tecnologias

- .NET 10
- Blazor Web App
- Entity Framework Core
- SQLite
- Bootstrap 5
- Chart.js (via JSInterop)

## Funcionalidades

- **Landing Page** — hero section, países-sede, próximos jogos, ranking FIFA em gráfico de barras e chamada para o simulador
- **Jogos** — listagem ordenada por data com informações de grupo e estádio
- **Grupos** — exibição dos grupos, classificação e estatísticas
- **Seleções** — informações, elencos e estatísticas dos jogadores (nome, posição, idade, gols, participações em copas)
- **Ranking FIFA** — exibição do ranking das seleções participantes
- **Simulador** — simulação de resultados da fase de grupos com persistência

## Estrutura do Projeto

```
src/
└── PortalCopa26/
    ├── PortalCopa26.slnx
    └── PortalCopa26/
        ├── Pages/
        ├── Components/
        ├── Models/
        ├── Services/
        └── Data/
```

## Como executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Executando localmente

```bash
cd src/PortalCopa26/PortalCopa26
dotnet run
```

A aplicação estará disponível em `https://localhost:5001`.

## Dados Iniciais

Os dados são carregados via Seed Data e incluem:

- Seleções e grupos
- Jogadores
- Jogos
- Ranking FIFA

## Fora do Escopo (v1)

- Área administrativa
- Autenticação / Autorização
- Gestão de usuários
- Integração com APIs externas
- Atualização automática de resultados
