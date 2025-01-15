namespace ConsultaNotas.DTOs.AvanceMateria
{
    public class SemestreAvanceMateriaDTO
    {
        public string Sigla { get; set; }
        public string Materia { get; set; }
        public int HorasPracticas { get; set; }
        public int HorasTeoricas { get; set; }
        public int Creditos { get; set; }
        public int Nota { get; set; }
        public string Periodo { get; set; }
    }
}
