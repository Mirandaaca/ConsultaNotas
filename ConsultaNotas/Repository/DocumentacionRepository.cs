using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using Dapper;
using System.Data;

namespace ConsultaNotas.Repository
{
    public class DocumentacionRepository : IDocumentacionRepository
    {
        private readonly IDbConnection _dbConnection;
        public DocumentacionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<Documentos> ObtenerDocumentosDeEstudiante(int registro)
        {
            string query = $"SELECT persona.doc6 AS ci, persona.doc2 AS tb, persona.doc5 AS cn, persona.doc7 AS foto " +
                $"FROM  acad.persona " +
                $"WHERE persona.registro = {registro}";
            return await _dbConnection.QueryFirstOrDefaultAsync<Documentos>(query);
        }
    }
}
