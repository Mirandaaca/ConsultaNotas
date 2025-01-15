using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using Dapper;
using System.Data;

namespace ConsultaNotas.Repository
{
    public class NotasRegimenRepository : INotasRegimenRepository
    {
        private readonly IDbConnection _dbconnection;
        public NotasRegimenRepository(IDbConnection dbConnection)
        {
            _dbconnection = dbConnection;
        }
        public async Task<NotasRegimen> ObtenerInformacionSemestre(int regimen)
        {
            var query = $"execute procedure SelectRegimen({regimen})";
            return await _dbconnection.QueryFirstOrDefaultAsync<NotasRegimen>(query);
        }
    }
}
