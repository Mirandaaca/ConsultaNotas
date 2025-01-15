using ConsultaNotas.DTOs.Historico;
using ConsultaNotas.DTOs.Semestre;
using ConsultaNotas.Entities;

namespace ConsultaNotas.Interfaces
{
    public interface INotasPeriodoRepository
    {
        public Task<IEnumerable<NotasPeriodo>> ObtenerPeriodosDeEstudiante(int registro);
        public Task<List<HistoricoDTO>> ObtenerHistoricoDeUnEstudiante(int registro);
        public Task<NotasYPeriodoDeUnSemestreDTO> ObtenerNotasDelSemestreActual(int registro);
        public Task<int> ObtenerPromedioSemestral(int ano, int semestre, int registro);
        public Task<int> ObtenerPromedioPonderadoAcumulado(int registro);
        public Task<int> ObtenerCreditosVencidos(int registro);
        public Task<int> ObtenerMateriasVencidas(int registro);
    }
}
