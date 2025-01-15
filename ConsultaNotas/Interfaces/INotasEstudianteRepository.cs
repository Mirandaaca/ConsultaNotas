using ConsultaNotas.DTOs.Auth;
using ConsultaNotas.Entities;

namespace ConsultaNotas.Interfaces
{
    public interface INotasEstudianteRepository
    {
        public Task<NotasEstudiante> ObtenerInformacionEstudiante(int registro);
        public Task<LogInEstudianteResponseDTO> LogIn(LogInEstudianteRequestDTO request);
    }
}
