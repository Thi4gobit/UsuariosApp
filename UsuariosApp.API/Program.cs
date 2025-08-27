using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Configura��o para que os endpoints da API sejam exibidos em letras min�sculas.
builder.Services.AddRouting(map => map.LowercaseUrls = true);

//Configura��o para habilitar a documenta��o do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();
app.MapControllers();
app.Run();



