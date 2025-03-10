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

    [HttpGet]
    [Route("salvar-dados-cep-informado")]
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
}