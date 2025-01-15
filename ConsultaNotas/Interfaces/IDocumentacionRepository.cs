using ConsultaNotas.Entities;

namespace ConsultaNotas.Interfaces
{
    public interface IDocumentacionRepository
    {
        public Task<Documentos> ObtenerDocumentosDeEstudiante(int registro);
    }
}
