using ConsultaNotas.DTOs.Historico;
using ConsultaNotas.DTOs.Semestre;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using Dapper;
using System.Data;

namespace ConsultaNotas.Repository
{
    public class NotasPeriodoRepository : INotasPeriodoRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly INotasSemestreRepository _notasSemestreRepository;

        public NotasPeriodoRepository(IDbConnection dbConnection, INotasSemestreRepository notasSemestreRepository )
        {
            _dbConnection = dbConnection;
            _notasSemestreRepository = notasSemestreRepository;
        }
        public async Task<IEnumerable<NotasPeriodo>> ObtenerPeriodosDeEstudiante(int registro)
        {
            var query = $"select * from west_ano_semlist where reg = {registro}";
            return await _dbConnection.QueryAsync<NotasPeriodo>(query);
        }
        public async Task<List<HistoricoDTO>> ObtenerHistoricoDeUnEstudiante(int registro)
        {
            IEnumerable<NotasPeriodo> periodosDelEstudiante = await ObtenerPeriodosDeEstudiante(registro);
            List<HistoricoDTO> notasDelEstudiante = new List<HistoricoDTO>();
            //Recorrer todos los periodos del estudiante para obtener las notas de ese semestre
            foreach (NotasPeriodo periodo in periodosDelEstudiante)
            {
                IEnumerable<NotasPeriodoHistorico> notaPeriodoHistorico = await ObtenerNotasDeUnPeriodoParaHistorico(periodo.ano, periodo.sem, registro);
                HistoricoDTO notaHistorico = new HistoricoDTO
                {
                    Periodo = periodo.periodo,
                    SemestreInfo = notaPeriodoHistorico.Select(nota => new SemestreHistoricoDTO
                    {
                        Sigla = nota.sigla,
                        Grupo = nota.grupo,
                        Creditos = nota.cr,
                        NombreMateria = nota.nombre_materia,
                        NotaFinal = nota.nota
                    }).ToList()
                };
                notasDelEstudiante.Add(notaHistorico);
            }
            return notasDelEstudiante;
        }
        public async Task<int> ObtenerPromedioSemestral(int ano, int semestre, int registro)
        {
            string query = $"execute procedure ProSemestral({ano}, {semestre}, {registro})";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(query);
        }
        public async Task<int> ObtenerPromedioPonderadoAcumulado(int registro)
        {
            string query = $"execute procedure promedioAcumulado({registro})";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(query);
        }
        public async Task<int> ObtenerCreditosVencidos(int registro)
        {
            string query = $"execute procedure creditosVencidos({registro})";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(query);
        }
        public async Task<int> ObtenerMateriasVencidas(int registro)
        {
            string query = $"execute procedure materiasVencidas({registro})";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(query);
        }
        public async Task<IEnumerable<NotasPeriodoHistorico>> ObtenerNotasDeUnPeriodoParaHistorico(int ano, string semestre, int registro)
        {
            string query = $"select materia.sigla, historico.grupo, materia.nombre_materia, materia.cr, historico.nota, historico.ano, historico.sem " +
                $"from historico, outer materia, est where (historico.reg = {registro}) and (materia.cod_mat = historico.cod_mat) " +
                $"and (historico.plan = materia.plan) and (historico.ano = {ano}) " +
                $"and (historico.sem = {semestre}) and (historico.reg = est.reg) and (historico.ok between '0' and '3') " +
                $"order by historico.ano, historico.sem, materia.carr, materia.sigla";
            return await _dbConnection.QueryAsync<NotasPeriodoHistorico>(query);
        }
        public async Task<NotasYPeriodoDeUnSemestreDTO> ObtenerNotasDelSemestreActual(int registro)
        {
            // Query para obtener el semestre actual
            var query = $"SELECT FIRST 1 DISTINCT regimen.cod_reg, trim (regimen.ano ||' - '|| regimen.nombre_reg) as periodo, regimen.ano, regimen.sem " +
        $"FROM acad.historico, acad.regimen WHERE historico.reg = {registro} and regimen.sem = historico.sem and regimen.ano = historico.ano " +
        $"ORDER BY regimen.cod_reg DESC ";

            // Obtener notas de el año y semestre actuales
            var semestreActual = await _dbConnection.QueryFirstOrDefaultAsync<NotasPeriodo>(query);

            if (semestreActual == default)
                throw new Exception("No se pudo obtener el semestre actual.");

            // Reutilizar el método ObtenerNotasDeUnSemestre
            return await _notasSemestreRepository.ObtenerNotasDeUnSemestre(registro, semestreActual.ano, int.Parse(semestreActual.sem));
        }
    }
}
