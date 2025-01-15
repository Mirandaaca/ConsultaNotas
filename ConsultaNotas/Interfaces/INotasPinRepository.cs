using ConsultaNotas.Entities;

namespace ConsultaNotas.Interfaces
{
    public interface INotasPinRepository
    {
        public Task<NotasPin> ObtenerPinEstudiante(int registro);
    }
}
