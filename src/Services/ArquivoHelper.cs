using Microsoft.Extensions.Configuration;
using Services.Wrappers;

namespace Services;
public class ArquivoHelper
{
    private const string PastaParaLogs = "dados";
    private string _pastaNoServidor;

    public ArquivoHelper()
    {
        _pastaNoServidor = Path.Combine(Directory.GetCurrentDirectory(), PastaParaLogs);
    }

    public bool ExcluirArquivo(string nomeDoArquivo)
    {
        var deletouArquivo = false;
        var caminhoDoArquivo = Path.Combine(_pastaNoServidor, nomeDoArquivo);

        if (VerificarSeArquivoExiste(caminhoDoArquivo))
        {
            File.Delete(caminhoDoArquivo);
            deletouArquivo = true;
        }

        return deletouArquivo;
    }

    public string SalvarDadosNoFormatoExperado(DadosCep dadosCep, string nomeDoArquivo = "dados_cep")
    {        
        var textoCep = $"Cep: {dadosCep.Cep} \r\n"
                     + $"Logradouro: {dadosCep.Logradouro} \r\n"
                     + $"Cidade: {dadosCep.Localidade} \r\n"
                     + $"Estado: {dadosCep.Uf} \r\n"
                     + "----------";

        var arquivoJaExiste = VerificarSeArquivoExiste(nomeDoArquivo);
        return CriarEditarArquivo(nomeDoArquivo, textoCep, arquivoJaExiste);
    }

    public bool VerificarSeArquivoExiste(string nomeDoArquivo)
    {
        var caminhoDoArquivo = Path.Combine(_pastaNoServidor, nomeDoArquivo);
        return File.Exists(caminhoDoArquivo);
    }
    public string CriarEditarArquivo(string nomeDoArquivo, string conteudoDoArquivo, bool arquivoJaExiste)
    {
        var pastaNoServidor = CriarPastaDeLogsSeNecessario();
        var caminhoDoArquivo = Path.Combine(pastaNoServidor, nomeDoArquivo);

        using (var writer = new StreamWriter(caminhoDoArquivo, append: arquivoJaExiste))
        {
            try
            {
                writer.WriteLine(conteudoDoArquivo);
                return caminhoDoArquivo;
            }
            finally
            {
                writer.Close();
            }
        }
    }

    public string[] ExtrairLinhasDoArquivo(string caminhoDoArquivo)
    {
        var conteudo = File.ReadAllText(caminhoDoArquivo);
        var dadosCep = conteudo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        return dadosCep;
    }
    
    public string CriarPastaDeLogsSeNecessario()
    {
        var pastaNoServidor = Path.Combine(Directory.GetCurrentDirectory(), _pastaNoServidor);

        if (!Directory.Exists(pastaNoServidor))
            Directory.CreateDirectory(pastaNoServidor);

        return pastaNoServidor;
    }
}