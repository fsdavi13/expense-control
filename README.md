# 💰 Expense Control

Sistema web para controle de gastos residenciais, desenvolvido com ASP.NET Core, React e TypeScript.

A aplicação permite cadastrar moradores, registrar receitas e despesas e acompanhar os totais financeiros individuais e gerais da residência.

## 🚀 Funcionalidades

### Pessoas

* Cadastro de pessoas com nome e idade;
* Listagem das pessoas cadastradas;
* Edição de nome e idade, preservando o identificador;
* Exclusão de pessoas;
* Exclusão automática das transações associadas à pessoa removida.

### Transações

* Cadastro de receitas e despesas;
* Associação obrigatória da transação a uma pessoa existente;
* Listagem das transações cadastradas;
* Validação de descrição e valor;
* Bloqueio de receitas para menores de 18 anos.
* Exclusão individual de transações com confirmação;

### Totais

* Total de receitas por pessoa;
* Total de despesas por pessoa;
* Saldo individual;
* Total geral de receitas;
* Total geral de despesas;
* Saldo geral da residência.

## 📋 Regras de negócio

* Toda transação deve estar associada a uma pessoa cadastrada;
* O valor da transação deve ser maior que zero;
* Pessoas menores de 18 anos podem registrar somente despesas;
* Uma pessoa que possui receitas não pode ter sua idade alterada para menos de 18 anos;
* Ao excluir uma pessoa, todas as suas transações também são excluídas;
* A edição de uma pessoa mantém o mesmo identificador e suas transações existentes.

## 🛠️ Tecnologias utilizadas

### Backend

* .NET 8
* ASP.NET Core Web API
* C#
* Entity Framework Core
* SQLite
* Swagger
* xUnit
* Moq

### Frontend

* React
* TypeScript
* Vite
* Axios
* CSS

## 🧱 Arquitetura

O backend foi dividido em camadas para separar responsabilidades:

```text
backend/
├── ExpenseControl.Api
├── ExpenseControl.Application
├── ExpenseControl.Domain
├── ExpenseControl.Infrastructure
└── ExpenseControl.Tests
```

### Domain

Contém as entidades, enumerações e contratos dos repositórios.

### Application

Contém os serviços, DTOs, interfaces, validações e regras de negócio.

### Infrastructure

Contém o acesso ao banco de dados, implementação dos repositórios e configuração do Entity Framework Core.

### Api

Contém os controllers, configuração da aplicação e tratamento centralizado de exceções.

### Tests

Contém os testes unitários das regras de negócio e dos serviços da aplicação.

O fluxo principal do backend segue:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
SQLite
```

## 📁 Estrutura do frontend

```text
frontend/
├── public/
├── src/
│   ├── components/
│   ├── pages/
│   ├── services/
│   ├── types/
│   └── utils/
├── .env.example
├── package.json
└── vite.config.ts
```

## ▶️ Como executar

### Pré-requisitos

Para executar o projeto localmente, é necessário ter instalado:

* [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0);
* [Node.js](https://nodejs.org/);
* npm, instalado junto com o Node.js;
* [Git](https://git-scm.com/).

O banco SQLite não precisa ser instalado separadamente.

### Primeira execução

Clone o repositório:

```bash
git clone https://github.com/fsdavi13/expense-control.git
cd expense-control
```

Instale as dependências do frontend:

```powershell
cd frontend
npm install
cd ..
```

Crie o arquivo de ambiente:

```powershell
Copy-Item frontend\.env.example frontend\.env
```

### Iniciar a aplicação

Na raiz do projeto, execute:

```powershell
.\start.ps1
```

O script iniciará automaticamente:

* a API ASP.NET Core;
* o frontend React.

A aplicação estará disponível em:

```text
http://localhost:5173
```

A documentação Swagger estará disponível em:

```text
http://localhost:5062/swagger
```

Para encerrar a aplicação, feche os dois terminais abertos pelo script.

## ⚙️ Variáveis de ambiente

O frontend utiliza a variável:

```env
VITE_API_URL=http://localhost:5062/api
```

O arquivo `.env.example` já contém a configuração necessária para execução local.

## 🔗 Endpoints principais

### Pessoas

```text
POST   /api/persons
GET    /api/persons
PUT    /api/persons/{id}
DELETE /api/persons/{id}
```

### Transações

```text
POST   /api/transactions
GET    /api/transactions
DELETE /api/transactions/{id}
```

### Totais

```text
GET /api/totals
```

## 🧪 Testes

Para executar os testes do backend:

```bash
cd backend
dotnet test
```

O projeto possui testes para as principais regras de negócio, incluindo:

* criação de pessoas;
* validação de nome e idade;
* edição de pessoas;
* preservação do identificador;
* bloqueio de receitas para menores;
* bloqueio da alteração de adulto com receitas para menor;
* criação de transações;
* exclusão de pessoas;
* cálculo dos totais.

## ✅ Validação do projeto

### Backend

```bash
cd backend
dotnet build
dotnet test
```

### Frontend

```bash
cd frontend
npm run lint
npm run build
```

## 💾 Persistência

Os dados são armazenados em um banco SQLite e permanecem disponíveis após o encerramento da aplicação.

O banco é criado e atualizado automaticamente durante a execução da API.

## 👨‍💻 Autor

Desenvolvido por [Davi Ferreira](https://github.com/fsdavi13).
