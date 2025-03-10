using System.Text.Json;

namespace Services.Wrappers;

public class CepApiWrapper
{        
    public DadosCep ObterDados(string cep)
    {
        var apiUrl = $"https://viacep.com.br/ws/{cep}/json";                

        using var httpClient = new HttpClient();
        var response = httpClient.GetAsync(apiUrl).Result;

        if (!response.IsSuccessStatusCode)        
            throw new HttpRequestException($"Erro na requisição: {response.StatusCode}");    

        var responseData = response.Content.ReadAsStringAsync().Result;
        var dadosCep = JsonSerializer.Deserialize<DadosCep>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });        
        return dadosCep;                
    }
}

public class DadosCep
{      
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Bairro { get; set; }
    public string? Localidade { get; set; }
    public string? Uf { get; set; }
}
