using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;
using Services;
using Services.Wrappers;

public class CepService {


    ArquivoHelper _arquivoHelper;
    CepApiWrapper _cepApiWrapper;
    public CepService()
    {
        _arquivoHelper = new ArquivoHelper();
        _cepApiWrapper = new CepApiWrapper();
    }

    public bool GravarDadosCep(string cep)
    {
        var dadosCep = _cepApiWrapper.ObterDados(cep);
        _arquivoHelper.SalvarDadosNoFormatoExperado(dadosCep);
        return true;
    }

    public DadosCep RetornarDadosCep(string cep) 
    {
        return _cepApiWrapper.ObterDados(cep);
    }

    public string RetornarDadosCepSalvos(){
        return _arquivoHelper.RetornarDadosSalvos();
    }

    public bool ValidarCep(string  cep)
    {
        return Regex.IsMatch(cep, @"^\d{8}$");
    }
}