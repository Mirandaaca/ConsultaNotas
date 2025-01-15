using ConsultaNotas.DTOs.Auth;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConsultaNotas.Repository
{
    public class NotasEstudianteRepository : INotasEstudianteRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly string clavesecreta;
        private readonly int duracionEnMinutos;
        public NotasEstudianteRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            clavesecreta = configuration.GetValue<string>("ApiSettings:SecretKey");
            duracionEnMinutos = configuration.GetValue<int>("ApiSettings:DurationInMinutes");
        }
        public async Task<NotasEstudiante> ObtenerInformacionEstudiante(int registro)
        {
            //Dapper se encarga de mapear los parametros al comando SQL, reemplazando @registro por el valor de la variable registro

            //Una forma de hacerlo
            //var query = "execute procedure SelectEstudiante(@registro)";
            //return await _dbConnection.QueryFirstOrDefaultAsync<NotasEstudiante>(query, new { registro });


            //Otra forma de hacerlo
            var query = $"execute procedure SelectEstudiante({registro})";
            return await _dbConnection.QueryFirstOrDefaultAsync<NotasEstudiante>(query);
        }
        public async Task<LogInEstudianteResponseDTO> LogIn(LogInEstudianteRequestDTO request)
        {
            var estudianteRegistroYPin = await _dbConnection.QueryFirstOrDefaultAsync<NotasPin>($"execute procedure SelectPin({request.Registro})");
            //Si el estudiante no existe o el pin es incorrecto, retornar un objeto LogInEstudianteResponseDTO con los valores null
            if (estudianteRegistroYPin is null || estudianteRegistroYPin.pin is null)
            {
                throw new Exception("Ese registro no existe");
            }
            if(int.Parse(estudianteRegistroYPin.pin) != request.Pin)
            {
                throw new Exception("Pin incorrecto");
            }
            //Obtener informacion adicional del estudiante para devolver junto con el JWT Token
            var estudianteInformacion = await ObtenerInformacionEstudiante(request.Registro);
            //Caso contrario, retornar un objeto LogInEstudianteResponseDTO con los valores correspondientes
            //Crear el JWT Token
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(clavesecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, estudianteRegistroYPin.registro.ToString()),
                    new Claim(ClaimTypes.Role, "Estudiante")
                }),
                Expires = DateTime.UtcNow.AddMinutes(duracionEnMinutos),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);
            //Si todo sale bien, retornar un objeto LogInEstudianteResponseDTO con los valores correspondientes
            LogInEstudianteResponseDTO estudianteDTO = new LogInEstudianteResponseDTO
            {
                InformacionEstudiante = new EstudianteLogInResponseDataDTO
                {
                    ano = estudianteInformacion.ano,
                    carrera = estudianteInformacion.carrera,
                    nombre = estudianteInformacion.nombre,
                    plan = estudianteInformacion.plan,
                    registro = estudianteInformacion.registro,
                    semestre = estudianteInformacion.semestre
                },
                Role = "Estudiante",
                JWTToken = manejadorToken.WriteToken(token)
            };
            return estudianteDTO;
        }
    }
}
