# Projeto Gabriel API

API RESTful desenvolvida em .NET 8 para gerenciar diversas entidades, incluindo Pessoas, Livros, Cartas Pokémon e um módulo completo de Finanças Pessoais. O projeto também inclui um frontend em React para interação com a API.

## ✨ Funcionalidades Principais

* **Autenticação e Autorização:** Sistema seguro de login e gerenciamento de usuários com tokens JWT.
* **Gerenciamento de Pessoas:** CRUD completo para entidades de Pessoa.
* **Gerenciamento de Livros:** CRUD completo para informações de Livros.
* **Gerenciamento de Cartas Pokémon:** CRUD para Cartas Pokémon, incluindo upload de imagens.
* **Módulo de Finanças Pessoais:**
    * Gerenciamento de Cartões de Crédito/Débito.
    * Gerenciamento de Categorias de Despesas/Receitas.
    * Registro e acompanhamento de Lançamentos financeiros.
    * Criação e gerenciamento de Parcelamentos.
    * Associação de Contas a Pessoas.
* **Upload de Arquivos:** Funcionalidade para upload e download de arquivos.
* **Logging Detalhado:** Registro de atividades e erros da aplicação.
* **Hypermedia (HATEOAS):** Links dinâmicos nas respostas da API para facilitar a navegação.
* **Migrations de Banco de Dados:** Versionamento e gerenciamento da estrutura do banco de dados com Flyway.
* **Frontend Interativo:** Interface de usuário desenvolvida em React para consumir os recursos da API.

## 🚀 Tecnologias Utilizadas

**Backend:**
* .NET 8 (C#)
* ASP.NET Core
* Entity Framework Core (para ORM com MySQL)
* Flyway (para migrations de banco de dados)
* JWT (JSON Web Tokens) para autenticação
* Serilog (para logging)
* Swagger/OpenAPI (para documentação da API)
* Docker

**Frontend:**
* React
* Axios (para requisições HTTP)
* React Router DOM

**Banco de Dados:**
* MySQL

## ⚙️ Pré-requisitos

**Backend:**
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker](https://www.docker.com/get-started) (recomendado para facilitar a execução com o banco de dados)
* Um servidor MySQL (pode ser via Docker)

**Frontend:**
* [Node.js e npm](https://nodejs.org/)

## ▶️ Como Executar o Projeto

### 1. Backend (.NET API)

**Opção 1: Com Docker (Recomendado)**

O projeto está configurado para ser executado facilmente com Docker Compose. Este método iniciará a API .NET e um container MySQL pré-configurado.

1.  Clone o repositório:
    ```bash
    git clone [https://github.com/gabrielselim/Projeto_Gabriel.git](https://github.com/gabrielselim/Projeto_Gabriel.git)
    cd Projeto_Gabriel
    ```
2.  Suba os containers Docker:
    ```bash
    docker-compose up --build
    ```
    A API estará acessível em `http://localhost:8080` (ou a porta definida no `docker-compose.yml` e `launchSettings.json`). O Swagger estará disponível em `http://localhost:8080/swagger`.

**Opção 2: Localmente (Sem Docker para a API)**

1.  **Configure o Banco de Dados:**
    * Certifique-se de ter um servidor MySQL em execução.
    * Crie um banco de dados (ex: `projeto_gabriel_db`).
    * Atualize a string de conexão em `Projeto_Gabriel/appsettings.json` (e/ou `appsettings.Development.json`):
        ```json
        "MySQLConnectionString": "server=localhost;port=3306;database=projeto_gabriel_db;user=root;password=your_password"
        ```
2.  **Execute as Migrations (Flyway):**
    * As migrations são aplicadas automaticamente na inicialização da API se o Flyway estiver configurado para tal no `Program.cs`. Alternativamente, você pode rodar as migrations manualmente usando uma ferramenta compatível com Flyway ou aplicando os scripts SQL da pasta `Projeto_Gabriel/db/migrations` na ordem correta.
3.  **Execute a API:**
    Navegue até a pasta do projeto da API:
    ```bash
    cd Projeto_Gabriel
    ```
    Execute o comando:
    ```bash
    dotnet run
    ```
    A API estará acessível em `http://localhost:5059` e `https://localhost:7165` (ou as portas definidas em `Projeto_Gabriel/Properties/launchSettings.json`). O Swagger estará disponível em `/swagger`.

### 2. Frontend (React)

1.  Navegue até a pasta do cliente:
    ```bash
    cd client
    ```
2.  Instale as dependências:
    ```bash
    npm install
    ```
3.  Execute a aplicação React:
    ```bash
    npm start
    ```
    O frontend estará acessível em `http://localhost:3000`.

## 📄 Documentação da API (Swagger)

Após iniciar o backend, a documentação interativa da API (Swagger UI) pode ser acessada através do endpoint:

* `/swagger`

Exemplo: `http://localhost:8080/swagger` (se usando Docker) ou `http://localhost:5059/swagger` (se localmente).

## 🏗️ Estrutura do Projeto (Simplificada)
```text
.
├── .github/workflows/         # GitHub Actions (CI/CD para Docker)
├── Projeto_Gabriel.Application/ # Camada de Aplicação (DTOs, Converters, Business Logic Interfaces, Hypermedia)
├── Projeto_Gabriel.Domain/      # Camada de Domínio (Entidades, Repositórios Interfaces, Serviços de Domínio)
├── Projeto_Gabriel.Infrastructure/ # Camada de Infraestrutura (Contexto DB, Repositórios Implementações, Migrations Mappings)
├── Projeto_Gabriel.Shared/      # Configurações compartilhadas (ex: TokenConfiguration)
├── Projeto_Gabriel/             # Projeto principal da API (Controllers, Program.cs, appsettings.json)
│   ├── db/                    # Scripts SQL (Migrations e Datasets)
│   └── Properties/            # Configurações de inicialização
├── client/                    # Aplicação Frontend em React
├── docker-compose.yml         # Configuração do Docker Compose
└── README.md                  # Este arquivo
```

## ☁️ Workflow Docker (CI)

O projeto possui um workflow configurado em `.github/workflows/docker-image.yml` que automatiza o build e o push de uma imagem Docker da API para um registro de container (configurado para o GitHub Packages, mas pode ser adaptado).


## ✍️ Autor

**Gabriel Sanz**

* **Portfólio Profissional:** [gabrielsanztech.com.br](https://gabrielsanztech.com.br/)
* **Documentação da API (Online):** [Ver API no Swagger](https://api.gabrielsanztech.com.br/swagger/index.html)
* **GitHub:** [@gabrielselim](https://github.com/gabrielselim) * **LinkedIn:** [Gabriel Sanz](https://www.linkedin.com/in/gabriel-sanz/)

