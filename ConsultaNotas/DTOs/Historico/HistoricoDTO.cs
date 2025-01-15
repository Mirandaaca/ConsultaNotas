namespace ConsultaNotas.DTOs.Historico
{
    public class HistoricoDTO
    {
        public string Periodo { get; set; }
        public List<SemestreHistoricoDTO> SemestreInfo { get; set; }
    }
}
