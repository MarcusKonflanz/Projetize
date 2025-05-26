# 🚀 Projetize – Transforme ideias em entregas

**Projetize** é um sistema de gerenciamento de tarefas e projetos colaborativos, voltado para equipes que desejam transformar ideias em ações organizadas, com foco em produtividade e clareza de entregas.

Este é um projeto full stack desenvolvido com **ASP.NET Core (API)**, **Blazor WebAssembly (Frontend)** e **banco de dados SQL**, com o objetivo de aprendizado prático e evolução profissional.

---

## ✨ Funcionalidades previstas

- ✅ Cadastro e login de usuários
- ✅ Criação e gerenciamento de projetos
- ✅ Tarefas com status e prazos
- ✅ Organização por equipes
- ✅ Interface intuitiva com MudBlazor
- ✅ Dashboard com indicadores (em breve)
- ✅ Notificações e comentários (em breve)

---

## 🧱 Tecnologias utilizadas

| Camada        | Tecnologia                        | Estado             |
|---------------|---------------------------------|--------------------|
| Backend       | ASP.NET Core 8 (Web API)         | Em desenvolvimento |
| Frontend      | Blazor WebAssembly + MudBlazor   | Futuro             |
| Banco de Dados| SQL Server                      | Futuro             |
| Autenticação  | JWT                             | Futuro             |
| Hospedagem    | GitHub + Render                 | Futuro             |

---

## 🚧 Status do Projeto

Em desenvolvimento inicial. Ainda não está em produção.  
Foco atual: estrutura base, autenticação e design da interface.

---

## 📝 Resumo do Projeto Full Stack Tasker — Status Atual

### Backend (API ASP.NET Core)
- Estruturação inicial da API com controllers REST para gerenciamento de tarefas.
- Implementação do sistema de autenticação com JWT, incluindo:
  - Cadastro e login de usuários.
  - Refresh token.
  - Validação e revogação de tokens.
- Validações automáticas com FluentValidation.
- Integração com banco de dados SQL Server (desenvolvimento) e SQLite (produção).
- Serviço em background para limpeza de tokens expirados.
- Configuração de CORS e Swagger com suporte a autenticação JWT.
- Logs básicos e tratamento de erros (se aplicável).

### Frontend (Blazor WebAssembly)
- Estrutura inicial do projeto Blazor WebAssembly.
- Configuração e uso do MudBlazor para UI.
- Implementação do sistema de autenticação no frontend, consumindo API JWT.
- Serviços para gerenciamento de sessão e armazenamento local com Blazored.LocalStorage.
- Comunicação HTTP configurada com HttpClient apontando para a API.
- Componentes principais para exibição e manipulação das tarefas.

### Próximos passos recomendados
- Implementação das funcionalidades CRUD completas para tarefas (backend + frontend).
- Melhoria na experiência do usuário (UX/UI) com MudBlazor.
- Testes unitários e integração.
- Implementação de notificações em tempo real (ex: SignalR) para atualizações de tarefas.
- Documentação adicional e pipelines CI/CD.

---

## 🤝 Contribuição

Este projeto é pessoal, mas será documentado e mantido com cuidado. Se você quiser acompanhar, sugerir algo ou contribuir, sinta-se à vontade para abrir issues ou forks.

---

## 📄 Licença

Este projeto é livre para estudo e aprendizado. Licenciamento futuro será definido após a primeira versão estável.

---

## Dependências

| Dependência                   | Tecnologia                       | Comando                                                               |
|------------------------------|---------------------------------|-------------------------------------------------------------------------|
| Entity Framework SQLServer    | Entity SQLServer               | dotnet add package Microsoft.EntityFrameworkCore.SqlServer              |
| Entity Framework SQLite       | Entity SQLite                  | dotnet add package Microsoft.EntityFrameworkCore.Sqlite                 |
| Entity Framework Tools        | Entity Tools                   | dotnet add package Microsoft.EntityFrameworkCore.Tools                  |
| Validação de Entrada          | FluentValidation               | dotnet add package FluentValidation.AspNetCore                          |
| Mapear DTOs (entidades)       | AutoMapper                     | dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection  |
| Swagger                       | Swashbuckle                    | dotnet add package Swashbuckle.AspNetCore                               |
| Autenticação com JWT          | JWT                            | dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer        |
| FluentValidation              |                                | dotnet add package FluentValidation.AspNetCore                          |
| BCrypt                        | Encripta senhas                | dotnet add package BCrypt.Net-Next                                      |

---

## 📌 Autor

Desenvolvido por Marcus Konflanz

---

## 📂 Estrutura do projeto

```bash
/Projetize
├── Projetize.sln
├── Projetize.Api      # API ASP.NET Core
└── Projetize.App      # Frontend Blazor WebAssembly
