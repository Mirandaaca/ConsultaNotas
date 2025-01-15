using ConsultaNotas.DTOs.Historico;
using ConsultaNotas.DTOs.Semestre;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace ConsultaNotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotasPeriodoController : ControllerBase
    {
        private readonly INotasPeriodoRepository _notasPeriodoRepository;
        public NotasPeriodoController(INotasPeriodoRepository notasPeriodoRepository)
        {
            _notasPeriodoRepository = notasPeriodoRepository;
        }
        [HttpGet("ObtenerPeriodosDeEstudiante")]
        public async Task<ActionResult<IEnumerable<NotasPeriodo>>> ObtenerPeriodosDeEstudiante(int registro)
        {
            try
            {
                var response = await _notasPeriodoRepository.ObtenerPeriodosDeEstudiante(registro);
                return Ok(new Response<IEnumerable<NotasPeriodo>>(message: "Periodos obtenidos correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [OutputCache(PolicyName = "NotasHistoricoPolicy")]
        [HttpGet("ObtenerHistoricoDeUnEstudiante")]
        public async Task<ActionResult<List<HistoricoDTO>>> ObtenerHistoricoDeUnEstudiante(int registro)
        {
            try
            {
                var response = await _notasPeriodoRepository.ObtenerHistoricoDeUnEstudiante(registro);
                return Ok(new Response<List<HistoricoDTO>>(message: "Historico obtenido correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [OutputCache(PolicyName = "NotasPeriodoPolicy")]
        [HttpGet("ObtenerNotasDelSemestreActual")]
        public async Task<ActionResult<NotasYPeriodoDeUnSemestreDTO>> ObtenerNotasDelSemestreActual(int registro)
        {
            try
            {
                var response = await _notasPeriodoRepository.ObtenerNotasDelSemestreActual(registro);
                return Ok(new Response<NotasYPeriodoDeUnSemestreDTO>(
                    message: "Notas del semestre actual obtenidas correctamente",
                    data: response
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(
                    message: ex.Message,
                    succeded: false
                ));
            }
        }
        [HttpGet("ObtenerPromedioSemestral")]
        public async Task<ActionResult<int>> ObtenerPromedioSemestral(int ano, int semestre, int registro)
        {
            try
            {
                var response = await _notasPeriodoRepository.ObtenerPromedioSemestral(ano, semestre, registro);
                return Ok(new Response<int>(message: "Promedio semestral obtenido correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [HttpGet("ObtenerPromedioPonderadoAcumulado")]
        public async Task<ActionResult<int>> ObtenerPromedioPonderadoAcumulado(int registro)
        {
            try
            {
                var response = await _notasPeriodoRepository.ObtenerPromedioPonderadoAcumulado(registro);
                return Ok(new Response<int>(message: "Promedio ponderado acumulado obtenido correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [HttpGet("ObtenerCreditosVencidos")]
        public async Task<ActionResult<int>> ObtenerCreditosVencidos(int registro)
        {
            try
            {
                var response = await _notasPeriodoRepository.ObtenerCreditosVencidos(registro);
                return Ok(new Response<int>(message: "Creditos vencidos obtenidos correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [HttpGet("ObtenerMateriasVencidas")]
        public async Task<ActionResult<int>> ObtenerMateriasVencidas(int registro)
        {
            try
            {
                var response = await _notasPeriodoRepository.ObtenerMateriasVencidas(registro);
                return Ok(new Response<int>(message: "Materias vencidas obtenidas correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
    }
}
