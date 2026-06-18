## ADDED Requirements

### Requirement: Solução e projeto Blazor Web App criados
A solução SHALL existir como `PortalCopa26.slnx` em `src/PortalCopa26/` e SHALL conter um projeto Blazor Web App (.NET 10) chamado `PortalCopa26`.

#### Scenario: Estrutura física da solução
- **WHEN** o repositório é clonado
- **THEN** os arquivos `src/PortalCopa26/PortalCopa26.slnx` e `src/PortalCopa26/PortalCopa26/PortalCopa26.csproj` existem

#### Scenario: Aplicação inicializa sem erros
- **WHEN** `dotnet run` é executado no projeto `PortalCopa26`
- **THEN** a aplicação inicia na porta padrão sem erros de compilação ou runtime

### Requirement: Estrutura de pastas do projeto
O projeto SHALL conter as pastas `Pages/`, `Components/`, `Models/`, `Services/`, `Services/Interfaces/` e `Data/` dentro de `src/PortalCopa26/PortalCopa26/`.

#### Scenario: Pastas existem no projeto
- **WHEN** o projeto é criado
- **THEN** as pastas `Pages/`, `Components/`, `Models/`, `Services/`, `Services/Interfaces/`, e `Data/` existem na raiz do projeto

### Requirement: Dependências NuGet configuradas
O projeto SHALL referenciar `Microsoft.EntityFrameworkCore.Sqlite` e `Microsoft.EntityFrameworkCore.Design` compatíveis com .NET 10.

#### Scenario: Build bem-sucedido com dependências
- **WHEN** `dotnet build` é executado
- **THEN** build conclui com código 0 e sem warnings de versão de pacote
