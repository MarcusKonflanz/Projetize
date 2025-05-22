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
|---------------|-----------------------------------|---------------------
| Backend       | ASP.NET Core 8 (Web API)          | Em desenvolvimento |
| Frontend      | Blazor WebAssembly + MudBlazor    | Futuro             |
| Banco de Dados| SQL Server                        | Futuro             |
| Autenticação  | JWT                               | Futuro             |
| Hospedagem    | GitHub + Render                   | Futuro             |


---

## 🚧 Status do Projeto
Em desenvolvimento inicial. Ainda não está em produção.
Foco atual: estrutura base, autenticação e design da interface.

---

## 🤝 Contribuição
Este projeto é pessoal, mas será documentado e mantido com cuidado. Se você quiser acompanhar, sugerir algo ou contribuir, sinta-se à vontade para abrir issues ou forks.

---

## 📄 Licença
Este projeto é livre para estudo e aprendizado. Licenciamento futuro será definido após a primeira versão estável.

---

## Depndencias

| Depndencia                       | Tecnologia                        | Comando                                                               |
|----------------------------------|-----------------------------------|------------------------------------------------------------------------
| Entity Framewrok SQLServer       | Entity SQLServer                  |dotnet add package Microsoft.EntityFrameworkCore.SqlServer             |
| Entity Framewrok SQLite          | Entity SQLite                     |dotnet add package Microsoft.EntityFrameworkCore.Sqlite                |
| Entity Framewrok Tools           | Entity Tools                      |dotnet add package Microsoft.EntityFrameworkCore.Tools                 |
| Validação de Entrada             | FluentValidation                  |dotnet add package FluentValidation.AspNetCore                         |
| Mapear DTOs (entidades)          | AutoMapper                        |dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection |
| Swagger                          | Swashbuckle                       |dotnet add package Swashbuckle.AspNetCore                              |
| Autenticação com JWT             | JWT                               |dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer       |
| FluentValidation                 |                                   | dotnet add package FluentValidation.AspNetCore                        |
| BCrypt                           | Encripta senhas                   | dotnet add package BCrypt.Net-Next                                    |

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





