using Microsoft.Extensions.FileProviders;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); //Mostrar Endpoints dos Controllers no Swagger
builder.Services.AddSwaggerGen();

// Disponibiliza os endpoints dos controllers no Swagger

builder.Services.AddScoped<CepService>();

var app = builder.Build();

var caminhoDosArquivosDeLog = new ArquivoHelper().CriarPastaDeLogsSeNecessario();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(caminhoDosArquivosDeLog),
    RequestPath = "/dados"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); //Mapear rotas dos controllers

app.Run();

