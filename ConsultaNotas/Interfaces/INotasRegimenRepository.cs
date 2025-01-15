using ConsultaNotas.Entities;

namespace ConsultaNotas.Interfaces
{
    public interface INotasRegimenRepository
    {
        public Task<NotasRegimen> ObtenerInformacionSemestre(int regimen);
    }
}
