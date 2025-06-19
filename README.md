# Mentoria .NET | Backend

## Projeto: Serviço de Identidade (Identity)

Este repositório contém a implementação de um serviço de identidade/autenticação para um ecossistema de microserviços, utilizando .NET. O objetivo é fornecer autenticação, autorização e gerenciamento de usuários para aplicações como e-commerce, com arquitetura modular e boas práticas de desenvolvimento.

### Estrutura do Projeto

- **Modules/Identity.Application**: Camada de aplicação, contém serviços de aplicação e contratos para manipulação de usuários.
- **Modules/Identity.Domain**: Camada de domínio, define entidades, interfaces de repositório e regras de negócio.
- **Modules/Identity.Infrastructure.IoC**: Configuração de injeção de dependências (IoC) e bootstrapping dos módulos.
- **Modules/Identity.Infrastructure.Repositories**: Implementação dos repositórios de acesso a dados.
- **Services/Identity.API**: API REST para exposição dos endpoints de autenticação e gerenciamento de usuários.

### Tecnologias Utilizadas

- .NET 7+ (C#)
- ASP.NET Core Web API
- Injeção de Dependência (IoC)
- Repositórios e DDD (Domain-Driven Design)
- Docker (docker-compose para orquestração)

### Como Executar

1. **Pré-requisitos:**

   - .NET SDK 7 ou superior
   - Docker (opcional, para execução via docker-compose)

2. **Build e Execução Local:**

   ```powershell
   # Restaurar pacotes
   dotnet restore Identity.sln
   # Buildar a solução
   dotnet build Identity.sln
   # Executar a API
   dotnet run --project Services/Identity.API/Identity.API.csproj
   ```

3. **Execução com Docker Compose:**
   ```powershell
   docker-compose up --build
   ```

### Próximos Passos

- Documentar endpoints da API (Swagger ou similar)
- Implementar autenticação JWT
- Adicionar testes unitários e de integração
- Integrar com outros microserviços do ecossistema
- Documentar decisões de arquitetura (ADR)

### Documentação

- Veja a pasta `docs/` para diagramas e decisões de arquitetura.
- ADRs (Architecture Decision Records) estão sendo utilizados para registrar decisões importantes.

---

> Projeto em desenvolvimento para fins de estudo e mentoria. Contribuições e sugestões são bem-vindas!
