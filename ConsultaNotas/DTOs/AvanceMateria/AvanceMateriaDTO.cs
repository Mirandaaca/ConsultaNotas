namespace ConsultaNotas.DTOs.AvanceMateria
{
    public class AvanceMateriaDTO
    {
        public string Semestre { get; set; }
        public List<SemestreAvanceMateriaDTO> SemestreInfo { get; set; }
    }
}
