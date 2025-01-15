namespace ConsultaNotas.DTOs.Auth
{
    public class EstudianteLogInResponseDataDTO
    {
        public int registro { get; set; }
        public string semestre { get; set; }
        public int ano { get; set; }
        public string nombre { get; set; }
        public string carrera { get; set; }
        public string plan { get; set; }
    }
}
