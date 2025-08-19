# MeuProjetoMVC

Este é um projeto ASP.NET Core MVC com **Identity**, **área de Admin** e controle de acesso por roles.  
Permite login de usuários comuns e admin, com menus diferenciados e páginas exclusivas.

---

## Funcionalidades

- Cadastro e login de usuários via ASP.NET Identity.
- Controle de acesso por roles (Admin e Usuário).
- Área exclusiva para Admin:
  - Dashboard.
  - Gerenciamento de usuários.
- Menu dinâmico conforme o papel do usuário.
- Layout responsivo usando Bootstrap.
- Suporte a SQLite via Entity Framework Core.

---

## Estrutura do Projeto

/MyImageApp (Pasta Raiz do Projeto)
│
├── /Areas
│   └── /Admin
│       ├── /Controllers
│       │   └── DashboardController.cs
│       └── /Views
│           └── /Dashboard
│               ├── Index.cshtml
│               └── GerenciarUsuarios.cshtml
│
├── /Controllers
│   ├── AccountController.cs
│   └── HomeController.cs
│
├── /Views
│   └── /Shared
│       └── _Layout.cshtml
│
├── appsettings.json
│
└── Program.cs



---

# Como rodar

1. Clone o repositório:


git clone https://github.com/dcair2024/MyImageApp.git

2. Acesse a pasta do projeto e restaure os pacotes:

dotnet restore

3. Crie e atualize o banco de dados:

4. Rode a aplicação:

dotnet run

5.Acesse no navegador:

https://localhost:7295/







