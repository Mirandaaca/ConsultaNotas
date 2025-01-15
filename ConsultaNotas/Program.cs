using ConsultaNotas.Extensions;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Odbc;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurar CORS
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!.Split(",");

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.WithOrigins(origenesPermitidos)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//Configurar CACHE
builder.Services.AddOutputCache(opciones =>
{
    opciones.AddPolicy("NotasPeriodoPolicy", policyBuilder =>
    {
        policyBuilder
            .Cache()
            .Expire(TimeSpan.FromMinutes(5))
            .SetVaryByQuery("registro")
            .Tag("notasperiodo");
    });
    opciones.AddPolicy("NotasHistoricoPolicy", policyBuilder =>
    {
        policyBuilder
            .Cache()
            .Expire(TimeSpan.FromMinutes(5))
            .SetVaryByQuery("registro")
            .Tag("notashistorico");
    });
    opciones.AddPolicy("AvanceMateriasPolicy", policyBuilder =>
    {
        policyBuilder
            .Cache()
            .Expire(TimeSpan.FromMinutes(5))
            .SetVaryByQuery("registro")
            .Tag("avancemateria");
    });
});

//Configurar la conexion ODBC como un servicio para acceder a la base de datos de Informix en cualquier parte y momento de la aplicacion
builder.Services.AddScoped<IDbConnection>(db =>
{
    var connectionString = builder.Configuration.GetConnectionString("notas");
    return new OdbcConnection(connectionString);
});

//Agregar repositorios como servicios
builder.Services.AddScoped<INotasEstudianteRepository, NotasEstudianteRepository>();
builder.Services.AddScoped<INotasPeriodoRepository, NotasPeriodoRepository>();
builder.Services.AddScoped<INotasPinRepository, NotasPinRepository>();
builder.Services.AddScoped<INotasRegimenRepository, NotasRegimenRepository>();
builder.Services.AddScoped<INotasSemestreRepository, NotasSemestreRepository>();
builder.Services.AddScoped<IAvanceMateriaRepository, AvanceMateriaRepository>();
builder.Services.AddScoped<IDocumentacionRepository, DocumentacionRepository>();
//Agregar soporte para autentication (requerido) y autorizacion (de ser necesario)

var key = builder.Configuration.GetValue<string>("ApiSettings:SecretKey")!;
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtbearer =>
{
    jwtbearer.RequireHttpsMetadata = false; //Cambiar a true cuando se despliegue
    jwtbearer.SaveToken = true;
    jwtbearer.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false, //Quizas sea necesario cambiar a true cuando se despliegue
        ValidateAudience = false, //Quizas sea necesario cambiar a true cuando se despliegue
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Agregar el middleware para capturar los errores
app.UseErrorHandlingMiddleware();

//Utilizar CORS
app.UseCors();
//Utilizar CACHE
app.UseOutputCache();
//Utilizar autenticacion
app.UseAuthentication();



app.UseAuthorization();

app.MapControllers();

app.Run();
