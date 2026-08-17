# CRUD de Documentos Fiscais

API REST desenvolvida em .NET 8 para recebimento, processamento e gerenciamento de documentos fiscais eletrônicos, inicialmente com suporte a NF-e e estrutura preparada para futura integração com CT-e e NFS-e.

A aplicação permite receber arquivos XML fiscais, extrair seus principais dados, armazená-los em SQL Server e disponibilizar operações REST para consulta e gerenciamento dos documentos.

## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- C#
- Dapper
- SQL Server
- AutoMapper
- Swagger / OpenAPI
- XML / LINQ to XML

---

# Funcionalidades

A aplicação possui os seguintes recursos:

- Recebimento de arquivos XML fiscais;
- Processamento de XML de NF-e;
- Armazenamento dos dados no SQL Server;
- Armazenamento do XML original;
- Consulta de documento por ID;
- Listagem de documentos;
- Paginação;
- Filtros por CNPJ, UF e razão social;
- Atualização de documentos;
- Exclusão de documentos;
- Documentação da API através do Swagger.

## Endpoints

| Método | Endpoint | Descrição |
|---|---|---|
| POST | `/api/documentos` | Recebe e processa um XML fiscal |
| GET | `/api/documentos` | Lista documentos com paginação e filtros |
| GET | `/api/documentos/{id}` | Consulta um documento específico |
| PUT | `/api/documentos/{id}` | Atualiza um documento |
| DELETE | `/api/documentos/{id}` | Exclui um documento |

---

# Como executar a aplicação

## Pré-requisitos

Antes de executar a aplicação, é necessário ter instalado:

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 ou superior

Também é possível executar o projeto utilizando outra IDE compatível com .NET 8.

---

## 1. Clonar o projeto

```bash
git clone <URL_DO_REPOSITORIO>
```

Depois:

```bash
cd CrudNotasFiscais
```

---

## 2. Configurar o banco de dados

Crie um banco de dados no SQL Server.

Exemplo:

```sql
CREATE DATABASE NotasFiscais;
```

Depois execute o script de criação da tabela `DocumentoFiscal`.

Exemplo:

```sql
CREATE TABLE DocumentoFiscal
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    TipoDocumento VARCHAR(10) NOT NULL,

    ChaveAcesso VARCHAR(44) NULL,

    Numero INT NULL,

    Serie INT NULL,

    DataEmissao DATETIME2 NULL,

    CnpjEmitente VARCHAR(14) NOT NULL,

    RazaoSocialEmitente VARCHAR(200) NULL,

    CnpjDestinatario VARCHAR(14) NULL,

    RazaoSocialDestinatario VARCHAR(200) NULL,

    Uf CHAR(2) NULL,

    ValorTotal DECIMAL(18,2) NULL,

    XmlOriginal XML NOT NULL,

    DataImportacao DATETIME2 NOT NULL
        DEFAULT GETDATE()
);
```

---

## 3. Configurar a connection string

No arquivo `appsettings.json`, configure a conexão com o SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NotasFiscaisAPI;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Caso esteja utilizando usuário e senha:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NotasFiscais;User Id=usuario;Password=senha;TrustServerCertificate=True;"
  }
}
```

---

## 4. Restaurar os pacotes

No diretório do projeto:

```bash
dotnet restore
```

---

## 5. Executar a aplicação

```bash
dotnet run
```

Ou execute diretamente pelo Visual Studio.

Após iniciar a aplicação, acesse o Swagger pela URL exibida no terminal, normalmente:

```text
https://localhost:7224/swagger
```

A porta pode variar de acordo com a configuração do projeto.

---

# Como utilizar a API

## Upload de XML

O endpoint:

```http
POST /api/documentos
```

recebe o XML através de `multipart/form-data`.

No Swagger, selecione:

```text
arquivo → Escolher arquivo
```

e envie o XML fiscal.

A aplicação:

1. Recebe o arquivo;
2. Lê o conteúdo XML;
3. Identifica/processa o documento;
4. Extrai os dados fiscais;
5. Valida o conteúdo;
6. Armazena os dados no SQL Server;
7. Mantém o XML original armazenado.

---

## Listagem

```http
GET /api/documentos
```

A listagem utiliza paginação.

Exemplo:

```http
GET /api/documentos?pagina=1&tamanhoPagina=50
```

Também é possível utilizar filtros:

```http
GET /api/documentos?filtro=CE
```

ou:

```http
GET /api/documentos?filtro=12345678000199
```

A resposta possui informações sobre a página atual, quantidade de registros e total de páginas.

---

## Consulta por ID

```http
GET /api/documentos/1
```

Retorna os dados do documento correspondente ao ID informado.

Caso o documento não exista:

```text
404 Not Found
```

---

## Atualização

```http
PUT /api/documentos/1
```

Permite atualizar os dados permitidos de um documento existente.

---

## Exclusão

```http
DELETE /api/documentos/1
```

Remove o documento correspondente ao ID informado.

---

# Decisões de arquitetura

A aplicação foi organizada utilizando separação de responsabilidades entre as principais camadas:

```text
CrudNotasFiscais
│
├── Controllers
│
├── Application
│   ├── Interfaces
│   └── Services
│
├── Domain
│   ├── Entities
│   ├── DTOs
│   └── Genericos
│
└── Infrastructure
    └── Repositories
```

## Controller

Responsável pela comunicação HTTP com o cliente.

Suas responsabilidades incluem:

- receber requisições;
- validar informações básicas da requisição;
- receber arquivos XML;
- retornar os códigos HTTP adequados.

A regra de negócio não fica concentrada no Controller.

---

## Service

A camada de Service concentra as regras de negócio da aplicação.

Entre suas responsabilidades estão:

- processar documentos;
- utilizar os parsers de XML;
- realizar validações;
- coordenar Repository e demais componentes;
- converter entidades para DTOs através do AutoMapper.

---

## Repository

A comunicação com o SQL Server é concentrada nos Repositories.

Foi utilizado Dapper por ser uma solução simples e performática para acesso a dados, permitindo controle direto sobre as queries SQL.

As queries utilizam parâmetros para evitar SQL Injection.

Exemplo:

```csharp
await _connection.QueryAsync<DocFiscal>(
    sql,
    new { Filtro = filtro }
);
```

---

# Modelagem

A aplicação utiliza inicialmente uma tabela principal:

```text
DocumentoFiscal
```

Ela armazena os principais dados comuns aos documentos fiscais:

- tipo do documento;
- chave de acesso;
- número;
- série;
- data de emissão;
- CNPJ do emitente;
- razão social;
- CNPJ do destinatário;
- UF;
- valor total;
- XML original;
- data de importação.

A escolha por uma estrutura inicialmente simplificada permite atender ao escopo do desafio sem criar uma modelagem complexa.

O campo `XmlOriginal` permite preservar o documento recebido e possibilita que novos dados sejam extraídos futuramente sem perder a informação original.

---

# DTOs

As entidades utilizadas internamente pela aplicação não são expostas diretamente pela API.

Foram utilizados DTOs para separar:

```text
Entidade de domínio
        ↓
     AutoMapper
        ↓
       DTO
        ↓
      API
```

Essa abordagem reduz o acoplamento entre o modelo de persistência e o contrato público da API.

Também permite criar DTOs específicos para diferentes operações, como:

- listagem;
- detalhes;
- criação;
- atualização.

---

# Paginação e performance

A listagem utiliza paginação através do SQL Server:

```sql
OFFSET @Offset ROWS
FETCH NEXT @TamanhoPagina ROWS ONLY
```

Dessa forma, a API não precisa carregar todos os registros do banco para depois realizar a paginação em memória.


---

# Possíveis melhorias

Caso houvesse mais tempo para evolução do projeto, algumas melhorias seriam implementadas.

## Suporte completo a NF-e, CT-e e NFS-e

Atualmente a estrutura foi preparada para diferentes tipos de documentos.

A evolução poderia utilizar uma abstração:

```text
IXmlFiscalParser
│
├── NFeParser
├── CTeParser
└── NFSeParser
```

Dessa forma, cada tipo de documento possuiria seu próprio parser.

---

## Autenticação e autorização

Adicionar autenticação utilizando JWT ou outro mecanismo de identidade.

Também poderiam ser implementados diferentes níveis de acesso para operações como:

- consulta;
- atualização;
- exclusão;
- upload.

---

## Melhor tratamento de erros

Implementar um middleware global de tratamento de exceções para padronizar as respostas da API.

Exemplo:

```json
{
    "status": 400,
    "message": "XML fiscal inválido",
    "errors": []
}
```

---

## Validação com FluentValidation

Utilizar FluentValidation para centralizar regras de validação dos DTOs e parâmetros da API.

---

## Armazenamento de arquivos

Em uma aplicação de produção, o XML original poderia ser armazenado em um serviço de armazenamento de objetos, como Azure Blob Storage ou Amazon S3, mantendo no SQL Server apenas os metadados e a referência para o arquivo.

Isso reduziria o volume de dados armazenado diretamente no banco.

---

## Testes automatizados

Adicionar:

- testes unitários;
- testes de integração;
- testes dos parsers;
- testes dos endpoints;
- testes de validação dos XMLs.

Principalmente testes para garantir que diferentes estruturas de NF-e sejam processadas corretamente.

---

## Docker

Adicionar Docker e Docker Compose para facilitar a configuração do ambiente de desenvolvimento.

A aplicação poderia ser executada com:

```bash
docker compose up
```

eliminando a necessidade de configurar manualmente todos os componentes do ambiente.


