using ConsultaNotas.DTOs.Semestre;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using Dapper;
using System.Data;

namespace ConsultaNotas.Repository
{
    public class NotasSemestreRepository : INotasSemestreRepository
    {
        private readonly IDbConnection _dbConnection;
        public NotasSemestreRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<NotasYPeriodoDeUnSemestreDTO> ObtenerNotasDeUnSemestre(int registro, int ano, int semestre)
        {
            // Query para obtener el período del semestre, esto para mostrarlo en el response que se enviara al frontend
            var queryPeriodo = $"SELECT DISTINCT trim (regimen.ano ||' - '|| regimen.nombre_reg) as periodo FROM acad.regimen WHERE regimen.ano = {ano} AND regimen.sem = {semestre}";

            // Obtener el período del semestre
            var periodo = await _dbConnection.QueryFirstOrDefaultAsync<string>(queryPeriodo);

            if (string.IsNullOrEmpty(periodo))
                throw new Exception("No se pudo obtener el período del semestre.");

            // Query para obtener las notas del semestre
            var queryNotas = $"SELECT * FROM wnotas_periodo WHERE reg = {registro} AND ano = {ano} AND sem = {semestre}";

            // Obtener las notas del semestre
            var notasSemestre = await _dbConnection.QueryAsync<NotasSemestre>(queryNotas);

            // Mapear las entidades a DTOs
            List<NotaSemestreDTO> notasSemestreDTOs = notasSemestre.Select(nota => new NotaSemestreDTO
            {
                CR = nota.cr,
                EC = nota.ec,
                EF = nota.ef,
                Grupo = nota.grupo,
                IE = nota.ie,
                NF = nota.nf,
                NombreMateria = nota.nombre_materia.Trim(),
                PP = nota.pp,
                Sigla = nota.sigla,
                SP = nota.sp,
                TP = nota.tp
            }).ToList();

            // Construir el DTO final, adjuntando el período y las notas del semestre
            NotasYPeriodoDeUnSemestreDTO historicoNotasDTO = new NotasYPeriodoDeUnSemestreDTO
            {
                Periodo = periodo,
                Notas = notasSemestreDTOs
            };

            return historicoNotasDTO;
        }
    }
}
