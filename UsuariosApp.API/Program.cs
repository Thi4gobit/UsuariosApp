using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using UsuariosApp.Domain.Interfaces.Repositories;
using UsuariosApp.Domain.Interfaces.Security;
using UsuariosApp.Domain.Interfaces.Services;
using UsuariosApp.Domain.Services;
using UsuariosApp.Infra.Data.Contexts;
using UsuariosApp.Infra.Data.Repositories;
using UsuariosApp.Infra.Security.Services;
using UsuariosApp.Infra.Security.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Configura��o para que os endpoints da API sejam exibidos em letras min�sculas.
builder.Services.AddRouting(map => map.LowercaseUrls = true);

//Configura��o para habilitar a documenta��o do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inje��o de depend�ncia da classe DataContext do EntityFramework
//capturando a connectionstring do arquivo /appsettings.json
builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(DataContext)));
});

//Inje��o de depend�ncia para capturar as configura��es do JWT mapeadas no arquivo /appsettings.json
builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(JwtBearerSettings))
       .Get<JwtBearerSettings>());

//Inje��o de depend�ncia para as demais interfaces e classes do projeto
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IJwtBearerSecurity, JwtBearerSecurity>();

//Configurar a politica de autentica��o do projeto
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    //configura��es para validar o token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, //validando a expira��o do token
        ValidateIssuerSigningKey = true, //valida a chave de assinatura do token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (builder.Configuration.GetValue<string>("JwtBearerSettings:SecretKey")))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Executar a documenta��o do Swagger
app.UseSwagger();
app.UseSwaggerUI();

//Executar a documenta��o do Scalar
app.MapScalarApiReference(options =>
{
    options.WithTheme(ScalarTheme.BluePlanet);
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

//Configurando a classe Program.cs como p�blica
public partial class Program { }

