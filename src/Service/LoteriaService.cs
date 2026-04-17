using Loteria.API.DTO;
using Loteria.API.Util;
using Newtonsoft.Json;

namespace Loteria.API.Service
{
    public class LoteriaService : ILoteriaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoteriaService> _logger;

        private const String BASE_URL = "https://servicebus2.caixa.gov.br/portaldeloterias/api/";

        public LoteriaService(HttpClient httpClient, ILogger<LoteriaService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<LoteriaDTO> ObterPelaLoteriaEConcurso(String Loteria, String Concurso)
        {
            String url = (!String.IsNullOrEmpty(Concurso) && Concurso.ToUpper() == Constantes.ULTIMO)  ? String.Concat(BASE_URL, Loteria) : String.Concat(BASE_URL, Loteria, "/", Concurso);

            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    String Resposta = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogInformation("Resultado da loteria {Loteria} concurso {Concurso} obtido na API externa (status {Status}).", Loteria, Concurso, (int)httpResponseMessage.StatusCode);
                    return JsonConvert.DeserializeObject<LoteriaDTO>(Resposta);
                }

                String corpo = await httpResponseMessage.Content.ReadAsStringAsync();
                _logger.LogWarning("API externa da Caixa retornou status {Status} para {Url}. Corpo: {Corpo}", (int)httpResponseMessage.StatusCode, url, corpo);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao chamar API externa da Caixa em {Url}", url);
                return null;
            }
        }
    }
}
