using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using Dapper;
using System.Data;

namespace ConsultaNotas.Repository
{
    public class NotasPinRepository : INotasPinRepository
    {
        private readonly IDbConnection _dbConnection;
        public NotasPinRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<NotasPin> ObtenerPinEstudiante(int registro)
        {
            var query = $"execute procedure SelectPin({registro})";
            return await _dbConnection.QueryFirstOrDefaultAsync<NotasPin>(query);
        }
    }
}
