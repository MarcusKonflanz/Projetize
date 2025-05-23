// Usings principais
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Projetize.Api.Data;
using Projetize.Api.Validators;
using Projetize.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
//  CONFIGURAÇÃO DE SERVIÇOS (Dependency Injection)
// Controllers e Endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger (documentação e autenticação via JWT no Swagger)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Projetize API",
        Version = "v1"
    });

    // Define o esquema de autenticação Bearer para o Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Informe o token JWT no formato: Bearer {seu_token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation(); // Ativa a validação automática
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserUpdateDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();

//  JWT Authentication
builder.Services.AddScoped<JwtService>(); // Serviço de geração de tokens

// SendGrid
builder.Services.AddScoped<IEmailService, EmailService>(); // Serviço de envio de e-mails

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // Verificação de tokens revogados
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDBContext>();
            var jwtToken = context.SecurityToken as JwtSecurityToken;

            if (jwtToken == null)
            {
                context.Fail("Token inválido");
                return;
            }

            var jti = jwtToken.Id;
            var tokenValue = !string.IsNullOrEmpty(jti) ? jti : jwtToken.RawData;

            var revoked = await dbContext.RevokedTokens.AnyAsync(rt => rt.Token == tokenValue);
            if (revoked)
            {
                context.Fail("Token revogado.");
            }
        }
    };
});

//  Banco de Dados
builder.Services.AddDbContext<AppDBContext>(options =>
{
#if DEBUG
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")); // Em modo dev, usa SQL Server
#else
    options.UseSqlite("Data Source=projetize.db"); // Em produção, usa SQLite
#endif
});

// Serviço em segundo plano para limpar tokens expirados
builder.Services.AddHostedService<TokenCleanupService>();

//  CONFIGURAÇÃO DO PIPELINE DE EXECUÇÃO (Middleware)

var app = builder.Build();

//  Middleware do Swagger em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar CORS
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});

app.UseCors();

app.UseHttpsRedirection();          // Redireciona HTTP -> HTTPS
app.UseAuthentication();           // Ativa autenticação JWT
app.UseAuthorization();            // Ativa autorização

app.MapControllers();              // Mapeia os endpoints dos controllers

app.Run();                         // Inicia a aplicação
