using System.Net;
using Microsoft.AspNetCore.Mvc;

[Route("api/cep")]
[ApiController]
public class CepController : ControllerBase
{
    CepService _cepService;
    public CepController(CepService cepService)
    {
        _cepService = cepService;
    }

    [HttpPost]
    [Route("salvar-dados-cep-informado/{cep}")]
    public ActionResult<IEnumerable<string>> SalvarDadosCepInformado(string cep)
    {
        try
        {
            if (string.IsNullOrEmpty(cep))
                return StatusCode((int)HttpStatusCode.BadRequest, "É necessario informar um Cep");

            if (!_cepService.ValidarCep(cep))
                return StatusCode((int)HttpStatusCode.BadRequest, "É necessario informar um Cep valido");

            _cepService.GravarDadosCep(cep);

            return StatusCode((int)HttpStatusCode.OK, new { mensagem = "Dados salvos com sucesso!" });

        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, ex.Message.ToString());
        }

    }

    [HttpGet]
    [Route("mostrar-dados-cep-no-formato-esperado/{cep}")]
    public IActionResult MostrarDadosDoCepNoFormatoEsperado(string cep)
    {
        try
        {
            if (string.IsNullOrEmpty(cep))
                return StatusCode((int)HttpStatusCode.BadRequest, "É necessario informar um Cep");

            if (!_cepService.ValidarCep(cep))
                return StatusCode((int)HttpStatusCode.BadRequest, "É necessario informar um Cep valido");

            var dadosCep = _cepService.RetornarDadosCep(cep);            

            return StatusCode((int)HttpStatusCode.OK, new { dadosCep });

        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, ex.Message.ToString());
        }
    }


    [HttpGet]
    [Route("mostrar-dados-cep-salvos")]
    public IActionResult MostrarDadosDoCepSalvos()
    {
        try
        {
            var result = _cepService.RetornarDadosCepSalvos();

            if(string.IsNullOrEmpty(result))
                return StatusCode((int)HttpStatusCode.OK, "Nenhum dado salvo");

            return Content(result, "text/plain");
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, ex.Message.ToString());
        }
    }
}