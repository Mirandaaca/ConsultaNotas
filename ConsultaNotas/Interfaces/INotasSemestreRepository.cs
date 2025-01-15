using ConsultaNotas.DTOs.Semestre;
using ConsultaNotas.Entities;

namespace ConsultaNotas.Interfaces
{
    public interface INotasSemestreRepository
    {
        public Task<NotasYPeriodoDeUnSemestreDTO> ObtenerNotasDeUnSemestre(int registro, int ano, int semestre);
    }
}
