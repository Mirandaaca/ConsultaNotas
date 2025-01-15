using ConsultaNotas.DTOs.AvanceMateria;
using ConsultaNotas.Entities;

namespace ConsultaNotas.Interfaces
{
    public interface IAvanceMateriaRepository
    {
        public Task<IEnumerable<AvanceMateriaSemestre>> CargarSemestresDeEstudiante(int registro);
        public Task<IEnumerable<AvanceMateria>> ObtenerNotasSemestre(int registro, int nsa);
        public Task<List<AvanceMateriaDTO>> ObtenerAvanceDeMaterias(int registro);

    }
}
