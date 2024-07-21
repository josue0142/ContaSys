using ContaSys.DTOs;
using ContaSys.Intefaces;
using Newtonsoft.Json;

namespace ContaSys.Services
{
    public class TasaCambiariaService : ITasaCambiariaService
    {
        private readonly HttpClient _httpClient;

        public TasaCambiariaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TasaCambiariaDTO>> ObtenerTasasCambiarias()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://neltonrg.pythonanywhere.com/api/tasacambiaria/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tasasCambiarias = JsonConvert.DeserializeObject<List<TasaCambiariaDTO>>(json);
                    return tasasCambiarias;
                }
                else
                {
                    Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
                    return new List<TasaCambiariaDTO>();
                }
            }
            catch (Exception)
            {
                return new List<TasaCambiariaDTO>();
            }
            
        }
    }
}
