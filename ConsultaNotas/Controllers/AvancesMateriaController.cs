using ConsultaNotas.DTOs.AvanceMateria;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace ConsultaNotas.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Estudiante")]
    [ApiController]
    public class AvancesMateriaController : ControllerBase
    {
        private readonly IAvanceMateriaRepository _avanceMateriaRepository;
        public AvancesMateriaController(IAvanceMateriaRepository avanceMateriaRepository)
        {
            _avanceMateriaRepository = avanceMateriaRepository;
        }
        [HttpGet("CargarSemestresDeEstudiante")]
        public async Task<ActionResult<IEnumerable<AvanceMateriaSemestre>>> CargarSemestresDeEstudiante(int registro)
        {
            try
            {
                var response = await _avanceMateriaRepository.CargarSemestresDeEstudiante(registro);
                return Ok(new Response<IEnumerable<AvanceMateriaSemestre>>(message: "Semestres cargados correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [HttpGet("ObtenerNotasSemestre")]
        public async Task<ActionResult<IEnumerable<AvanceMateria>>> ObtenerNotasSemestre(int registro, int nsa)
        {
            try
            {
                var response = await _avanceMateriaRepository.ObtenerNotasSemestre(registro, nsa);
                return Ok(new Response<IEnumerable<AvanceMateria>>(message: "Notas obtenidas correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [OutputCache(PolicyName = "AvanceMateriasPolicy")]
        [HttpGet("ObtenerAvanceMaterias")]
        public async Task<ActionResult<List<AvanceMateriaDTO>>> ObtenerAvanceMaterias(int registro)
        {
            try
            {
                var response = await _avanceMateriaRepository.ObtenerAvanceDeMaterias(registro);
                return Ok(new Response<List<AvanceMateriaDTO>>(message: "Avance de materias obtenido correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
    }
}
