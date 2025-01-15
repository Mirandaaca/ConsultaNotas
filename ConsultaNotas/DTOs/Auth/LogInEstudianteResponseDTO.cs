namespace ConsultaNotas.DTOs.Auth
{
    public class LogInEstudianteResponseDTO
    {
        public EstudianteLogInResponseDataDTO InformacionEstudiante { get; set; }
        public string Role { get; set; }
        public string JWTToken { get; set; }
    }
}
