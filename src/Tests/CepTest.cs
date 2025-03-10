using Services.Wrappers;
using Services;

namespace Tests;

[TestClass]
public class CepTest
{

    CepApiWrapper _cepApiWrapper;
    ArquivoHelper _arquivoHelper;
    CepService _cepService;

    [TestInitialize]
    public void Setup()
    {
        _cepApiWrapper = new CepApiWrapper();
        _arquivoHelper = new ArquivoHelper();
        _cepService = new CepService();
    }

    [TestMethod]
    public void DeveBuscarDadoesDeUmEndereco()
    {
        // //ARRANGE
        var cep = "01001-000";

        // //ACT
        var dadosCep = _cepApiWrapper.ObterDados(cep);

        // //ASSERT
        if (dadosCep.Cep != "01001-000")
            Assert.Fail();
        if (dadosCep.Logradouro != "Praça da Sé")
            Assert.Fail();
        if (dadosCep.Bairro != "Sé")
            Assert.Fail();
        if (dadosCep.Localidade != "São Paulo")
            Assert.Fail();
        if (dadosCep.Uf != "SP")
            Assert.Fail();

        Assert.IsTrue(true);
    }

    [TestMethod]
    public void DeveSalvarAquivoComDados()
    {
        //ARRANGE
        var cep = "79033470";
        var dadosCep = _cepApiWrapper.ObterDados(cep);

        //ACT        
        var caminhoDoArquivo = _arquivoHelper.SalvarDadosNoFormatoExperado(dadosCep, "dados_cep_test.txt");

        //ASSERT
        if (!_arquivoHelper.VerificarSeArquivoExiste(caminhoDoArquivo))
            Assert.Fail();

        _arquivoHelper.ExcluirArquivo(caminhoDoArquivo);
        Assert.IsTrue(true);
    }

    [TestMethod]
    public void DeveSalvarDadosDeMaisDeUmCep()
    {
        //ARRANGE
        var ceps = new List<string>() { "68909715", "01001000" };
        string caminhoDoArquivo = string.Empty;
        foreach (var cep in ceps)
        {
            //ACT        
            var dadosCep = _cepApiWrapper.ObterDados(cep);
            caminhoDoArquivo = _arquivoHelper.SalvarDadosNoFormatoExperado(dadosCep, "dados_cep_test.txt");
        }

        //ASSERT
        if (!_arquivoHelper.VerificarSeArquivoExiste(caminhoDoArquivo))
            Assert.Fail();

        var linhasDadosCep = _arquivoHelper.ExtrairLinhasDoArquivo(caminhoDoArquivo);

        if (linhasDadosCep[0].Split(":")[1].ToString().Trim() != "68909-715")
            Assert.Fail();

        if (linhasDadosCep[5].Split(":")[1].ToString().Trim() != "01001-000")
            Assert.Fail();

        _arquivoHelper.ExcluirArquivo(caminhoDoArquivo);

        Assert.IsTrue(true);
    }

    [TestMethod]
    public void DeveValidarOCep()
    {
        var cepInvalido = new List<string>() {
            "12.345-678",
            "1234 5678",
            "ABCDE123",
            "1234567",
            "123456789",
            "1234-56AB",
            "00000-000"
        };

        foreach (var cep in cepInvalido)
        {
            var cepEhValido = _cepService.ValidarCep(cep);

            if (cepEhValido)
                Assert.Fail();
        }

        Assert.IsTrue(true);
    }
}