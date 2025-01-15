namespace ConsultaNotas.DTOs.Semestre
{
    public class NotasYPeriodoDeUnSemestreDTO
    {
        public string Periodo { get; set; }
        public List<NotaSemestreDTO> Notas { get; set; }
    }
}
