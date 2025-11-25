# AMBEV Developer Evaluation - Backend API

API REST desenvolvida em .NET 8.0 para avaliação de desenvolvedores, implementando uma solução completa de e-commerce com gerenciamento de produtos, carrinho de compras, vendas e usuários.

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Execução](#instalação-e-execução)
- [Configuração](#configuração)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Funcionalidades](#funcionalidades)
- [Documentação da API](#documentação-da-api)
- [Testes](#testes)
- [Docker](#docker)

## 🎯 Sobre o Projeto

Este projeto é uma API RESTful desenvolvida seguindo os princípios de **Domain-Driven Design (DDD)** e **Clean Architecture**, implementando padrões como **CQRS** (Command Query Responsibility Segregation) e **MediatR** para separação de responsabilidades.

A aplicação gerencia:
- **Usuários**: Autenticação e autorização com JWT
- **Produtos**: CRUD completo com categorias e avaliações
- **Carrinhos de Compras**: Gerenciamento de itens e quantidades
- **Vendas**: Processamento de vendas com itens e regras de desconto

## 🛠 Tecnologias

### Framework e Linguagem
- **.NET 8.0** (C#)
- **ASP.NET Core Web API**

### Banco de Dados
- **PostgreSQL 13** (banco principal)
- **MongoDB 8.0** (NoSQL)
- **Redis 7.4.1** (cache)

### Bibliotecas Principais
- **Entity Framework Core 8.0.10** - ORM
- **MediatR 12.4.1** - Implementação de CQRS
- **AutoMapper** - Mapeamento de objetos
- **Swashbuckle.AspNetCore 6.8.1** - Documentação Swagger
- **Serilog** - Logging estruturado
- **JWT** - Autenticação e autorização

### Ferramentas
- **Docker** e **Docker Compose** - Containerização
- **Health Checks** - Monitoramento de saúde da aplicação

## 🏗 Arquitetura

O projeto segue uma arquitetura em camadas com separação clara de responsabilidades:

```
┌─────────────────────────────────────┐
│      WebApi (Apresentação)          │
│  - Controllers                       │
│  - Middleware                        │
│  - Mappings                          │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│   Application (Casos de Uso)         │
│  - Commands/Queries                  │
│  - Handlers                          │
│  - DTOs                              │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      Domain (Domínio)                │
│  - Entities                          │
│  - Value Objects                     │
│  - Specifications                    │
│  - Validation                        │
│  - Events                            │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      ORM (Infraestrutura)            │
│  - DbContext                         │
│  - Repositories                      │
│  - Migrations                        │
└──────────────────────────────────────┘
```

### Camadas

1. **WebApi**: Camada de apresentação com controllers, middleware e configurações HTTP
2. **Application**: Camada de aplicação com casos de uso (Commands/Queries) usando MediatR
3. **Domain**: Camada de domínio com entidades, regras de negócio e validações
4. **ORM**: Camada de infraestrutura com acesso a dados (Entity Framework Core)
5. **Common**: Componentes compartilhados (validação, segurança, logging, health checks)
6. **IoC**: Injeção de dependências e resolução de módulos

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (opcional, mas recomendado)
- [PostgreSQL 13+](https://www.postgresql.org/download/) (se não usar Docker)
- [Git](https://git-scm.com/)

## 🚀 Instalação e Execução

### Opção 1: Usando Docker Compose (Recomendado)

1. Clone o repositório:
```bash
git clone <url-do-repositorio>
cd backend
```

2. Execute o Docker Compose:
```bash
docker-compose up -d
```

Isso irá iniciar:
- API na porta `8080`
- PostgreSQL na porta `5432`
- MongoDB na porta `27017`
- Redis na porta `6379`

3. Acesse a documentação Swagger:
```
http://localhost:8080/swagger
```

### Opção 2: Execução Local

1. Configure a string de conexão no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=developer_evaluation;Username=seu_usuario;Password=sua_senha"
  }
}
```

2. Execute as migrações:
```bash
cd src/Ambev.DeveloperEvaluation.ORM
dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project ../Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj
```

3. Execute a aplicação:
```bash
cd src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

4. Acesse:
```
http://localhost:8080/swagger
```

## ⚙️ Configuração

### Variáveis de Ambiente

As principais configurações estão no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=ambev.developerevaluation.database;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
  },
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong"
  }
}
```

### Configurações do Docker

O `docker-compose.yml` configura automaticamente:
- **PostgreSQL**: Banco de dados principal
- **MongoDB**: Banco NoSQL
- **Redis**: Cache e sessões
- **WebApi**: Aplicação principal

## 📁 Estrutura do Projeto

```
backend/
├── src/
│   ├── Ambev.DeveloperEvaluation.WebApi/      # Camada de apresentação
│   │   ├── Features/                          # Endpoints organizados por feature
│   │   ├── Mappings/                          # AutoMapper profiles
│   │   ├── Middleware/                        # Middlewares customizados
│   │   └── Program.cs                         # Configuração da aplicação
│   │
│   ├── Ambev.DeveloperEvaluation.Application/ # Camada de aplicação
│   │   ├── Auth/                              # Autenticação
│   │   ├── Products/                          # Casos de uso de produtos
│   │   ├── Carts/                             # Casos de uso de carrinhos
│   │   ├── Sales/                             # Casos de uso de vendas
│   │   └── Users/                             # Casos de uso de usuários
│   │
│   ├── Ambev.DeveloperEvaluation.Domain/      # Camada de domínio
│   │   ├── Entities/                          # Entidades do domínio
│   │   ├── Enums/                             # Enumerações
│   │   ├── Events/                            # Eventos de domínio
│   │   ├── Repositories/                      # Interfaces de repositórios
│   │   ├── Services/                          # Serviços de domínio
│   │   ├── Specifications/                    # Especificações
│   │   └── Validation/                        # Validações de domínio
│   │
│   ├── Ambev.DeveloperEvaluation.ORM/          # Camada de infraestrutura
│   │   ├── DefaultContext.cs                  # DbContext do EF Core
│   │   ├── Mapping/                           # Configurações de mapeamento
│   │   ├── Migrations/                        # Migrações do banco
│   │   └── Repositories/                      # Implementação de repositórios
│   │
│   ├── Ambev.DeveloperEvaluation.Common/      # Componentes compartilhados
│   │   ├── HealthChecks/                      # Health checks
│   │   ├── Logging/                           # Configuração de logging
│   │   ├── Security/                          # Segurança e JWT
│   │   └── Validation/                        # Validações compartilhadas
│   │
│   └── Ambev.DeveloperEvaluation.IoC/         # Injeção de dependências
│       └── DependencyResolver.cs              # Resolução de dependências
│
├── tests/                                      # Projetos de teste
│   ├── Ambev.DeveloperEvaluation.Unit/        # Testes unitários
│   ├── Ambev.DeveloperEvaluation.Integration/ # Testes de integração
│   └── Ambev.DeveloperEvaluation.Functional/  # Testes funcionais
│
├── docker-compose.yml                         # Configuração Docker Compose
├── Dockerfile                                 # Dockerfile da aplicação
└── README.md                                  # Este arquivo
```

## 🔧 Funcionalidades

### Autenticação
- ✅ Autenticação de usuários com JWT
- ✅ Autorização baseada em roles
- ✅ Hash de senhas seguro

### Produtos
- ✅ CRUD completo de produtos
- ✅ Busca por categoria
- ✅ Sistema de avaliações (Rating)
- ✅ Validações de domínio

### Carrinhos de Compras
- ✅ Criar e gerenciar carrinhos
- ✅ Adicionar/remover produtos
- ✅ Atualizar quantidades
- ✅ Validações de negócio

### Vendas
- ✅ Processar vendas
- ✅ Gerenciar itens de venda
- ✅ Aplicar regras de desconto
- ✅ Cancelar vendas e itens
- ✅ Histórico de vendas

### Usuários
- ✅ CRUD de usuários
- ✅ Gestão de roles e status
- ✅ Validações de dados

## 📚 Documentação da API

A documentação interativa da API está disponível através do **Swagger** quando a aplicação está em execução:

- **URL**: `http://localhost:8080/swagger`
- **Ambiente**: Disponível apenas em modo Development

### Endpoints Principais

#### Autenticação
- `POST /api/auth/login` - Autenticar usuário

#### Produtos
- `GET /api/products` - Listar produtos
- `GET /api/products/{id}` - Obter produto por ID
- `GET /api/products/category/{category}` - Produtos por categoria
- `GET /api/products/categories` - Listar categorias
- `POST /api/products` - Criar produto
- `PUT /api/products/{id}` - Atualizar produto
- `DELETE /api/products/{id}` - Deletar produto

#### Carrinhos
- `GET /api/carts` - Listar carrinhos
- `GET /api/carts/{id}` - Obter carrinho por ID
- `POST /api/carts` - Criar carrinho
- `PUT /api/carts/{id}` - Atualizar carrinho
- `DELETE /api/carts/{id}` - Deletar carrinho

#### Vendas
- `GET /api/sales` - Listar vendas
- `GET /api/sales/{id}` - Obter venda por ID
- `POST /api/sales` - Criar venda
- `PUT /api/sales/{id}` - Atualizar venda
- `DELETE /api/sales/{id}` - Deletar venda
- `POST /api/sales/{id}/cancel` - Cancelar venda
- `POST /api/sales/{id}/items/{itemId}/cancel` - Cancelar item de venda

#### Usuários
- `GET /api/users/{id}` - Obter usuário por ID
- `POST /api/users` - Criar usuário
- `PUT /api/users/{id}` - Atualizar usuário
- `DELETE /api/users/{id}` - Deletar usuário

## 🧪 Testes

O projeto inclui três tipos de testes:

### Testes Unitários
```bash
cd tests/Ambev.DeveloperEvaluation.Unit
dotnet test
```

### Testes de Integração
```bash
cd tests/Ambev.DeveloperEvaluation.Integration
dotnet test
```

### Testes Funcionais
```bash
cd tests/Ambev.DeveloperEvaluation.Functional
dotnet test
```

### Gerar Relatório de Cobertura

**Windows:**
```bash
coverage-report.bat
```

**Linux/Mac:**
```bash
./coverage-report.sh
```

## 🐳 Docker

### Build da Imagem
```bash
docker build -t ambev-developer-evaluation-webapi -f src/Ambev.DeveloperEvaluation.WebApi/Dockerfile .
```

### Executar Container
```bash
docker run -p 8080:8080 ambev-developer-evaluation-webapi
```

### Docker Compose

Iniciar todos os serviços:
```bash
docker-compose up -d
```

Parar todos os serviços:
```bash
docker-compose down
```

Visualizar logs:
```bash
docker-compose logs -f ambev.developerevaluation.webapi
```

## 🔍 Health Checks

A aplicação inclui health checks configurados. Acesse:

- `http://localhost:8080/health` - Health check básico

## 📝 Logging

O projeto utiliza **Serilog** para logging estruturado. Os logs são configurados automaticamente e incluem:
- Informações de requisições HTTP
- Erros e exceções
- Operações de banco de dados
- Eventos de autenticação

## 🔐 Segurança

- **JWT**: Autenticação baseada em tokens
- **Hash de Senhas**: Senhas são hasheadas antes de serem armazenadas
- **Validação**: Validações em múltiplas camadas (Domain, Application, API)
- **CORS**: Configurável via `appsettings.json`

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto foi desenvolvido para fins de avaliação técnica.

## 👨‍💻 Autor

Desenvolvido para o processo de avaliação de desenvolvedores da AMBEV.

---

**Nota**: Este é um projeto de avaliação técnica. Para produção, considere:
- Configurar variáveis de ambiente adequadas
- Implementar HTTPS
- Configurar CORS adequadamente
- Adicionar rate limiting
- Implementar monitoramento e alertas
- Configurar backup do banco de dados
- Revisar políticas de segurança

