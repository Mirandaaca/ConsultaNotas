using ConsultaNotas.DTOs.AvanceMateria;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using Dapper;
using System.Data;

namespace ConsultaNotas.Repository
{
    public class AvanceMateriaRepository : IAvanceMateriaRepository
    {
        private readonly IDbConnection _dbConnection;
        public AvanceMateriaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<AvanceMateriaSemestre>> CargarSemestresDeEstudiante(int registro)
        {
            string query = $"SELECT DISTINCT(" +
                $"CASE WHEN (materia.nsa = '1') THEN 'Primer Semestre' " +
                $"WHEN (materia.nsa = '2') THEN 'Segundo Semestre' " +
                $"WHEN (materia.nsa = '3') THEN 'Tercer Semestre' " +
                $"WHEN (materia.nsa = '4') THEN 'Cuarto Semestre' " +
                $"WHEN (materia.nsa = '5') THEN 'Quinto Semestre' " +
                $"WHEN (materia.nsa = '6') THEN 'Sexto Semestre' " +
                $"WHEN (materia.nsa = '7') THEN 'Septimo Semestre' " +
                $"WHEN (materia.nsa = '8') THEN 'Octavo Semestre' " +
                $"WHEN (materia.nsa = '9') THEN 'Noveno Semestre' " +
                $"WHEN (materia.nsa = '10') THEN 'Decimo Semestre' " +
                $"WHEN (materia.nsa = '12') THEN 'Materias Electivas' " +
                $"WHEN (materia.nsa = '11') THEN 'Materias Tecnicas' ELSE '' END) AS semestre, materia.nsa from est, materia " +
                $"where est.reg = {registro} and materia.carr = est.carr and materia.plan = est.plan " +
                $"ORDER BY materia.nsa";
            return await _dbConnection.QueryAsync<AvanceMateriaSemestre>(query);
        }
        public async Task<IEnumerable<AvanceMateria>> ObtenerNotasSemestre(int registro, int nsa)
        {
            string query = $"SELECT(" +
                $"CASE WHEN (materia.nsa = '1') THEN 'Primer Semestre' " +
                $"WHEN (materia.nsa = '2') THEN 'Segundo Semestre' " +
                $"WHEN (materia.nsa = '3') THEN 'Tercer Semestre' " +
                $"WHEN (materia.nsa = '4') THEN 'Cuarto Semestre' " +
                $"WHEN (materia.nsa = '5') THEN 'Quinto Semestre' " +
                $"WHEN (materia.nsa = '6') THEN 'Sexto Semestre' " +
                $"WHEN (materia.nsa = '7') THEN 'Septimo Semestre' " +
                $"WHEN (materia.nsa = '8') THEN 'Octavo Semestre' " +
                $"WHEN (materia.nsa = '9') THEN 'Noveno Semestre' " +
                $"WHEN (materia.nsa = '10') THEN 'Decimo Semestre' " +
                $"WHEN (materia.nsa = '12') THEN 'Materias Electivas' " +
                $"WHEN (materia.nsa = '11') THEN 'Materias Tecnicas' " +
                $"ELSE '' END) AS semestre, materia.sigla, materia.nombre_materia, materia.hp, materia.ht, materia.cr, historico.nota, regimen.sem_romano AS sem, historico.ano ||'-' as ano from est, materia, outer (historico, regimen) " +
                $"where est.reg = {registro} and materia.carr = est.carr and materia.plan = est.plan and materia.a_b = 'A' and materia.nsa = '{nsa}' and historico.reg = est.reg and historico.nota > 50 and ((historico.ok between '0' and '3') or (historico.ok between 'A' and 'N')) and (materia.cod_mat = historico.cod_mat or historico.cod_mat = -99) and regimen.sem = historico.sem and regimen.ano = historico.ano " +
                $"ORDER BY materia.carr, materia.plan, materia.nsa, materia.sigla";
            return await _dbConnection.QueryAsync<AvanceMateria>(query);
        }
        public async Task<List<AvanceMateriaDTO>> ObtenerAvanceDeMaterias(int registro)
        {
            IEnumerable<AvanceMateriaSemestre> semestresEstudiante = await CargarSemestresDeEstudiante(registro);
            List<AvanceMateriaDTO> avanceMaterias = new List<AvanceMateriaDTO>();
            foreach (AvanceMateriaSemestre semestre in semestresEstudiante)
            {
                IEnumerable<AvanceMateria> materiasSemestre = await ObtenerNotasSemestre(registro, semestre.nsa);
                AvanceMateriaDTO avanceMateria = new AvanceMateriaDTO
                {
                    Semestre = semestre.semestre,
                    SemestreInfo = materiasSemestre.Select(nota => new SemestreAvanceMateriaDTO
                    {
                        Periodo = $"{nota.ano}{nota.sem}",
                        Creditos = nota.cr,
                        HorasPracticas = nota.hp,
                        HorasTeoricas = nota.ht,
                        Nota = nota.nota,
                        Materia = nota.nombre_materia,
                        Sigla = nota.sigla
                    }).ToList()
                };
                avanceMaterias.Add(avanceMateria);
            }
            return avanceMaterias;
        }
    }
}
