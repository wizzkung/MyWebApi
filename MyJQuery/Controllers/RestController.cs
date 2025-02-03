using Microsoft.AspNetCore.Mvc;
using MyJQuery.Model;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MyJQuery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public RestController(IHttpClientFactory httpClient) 
        {
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri("https://fakerestapi.azurewebsites.net/api/v1/Users");
        }
        [HttpGet, Route("getUsers")]
        public async Task<ActionResult> getUsers()
        {
            var response = await _httpClient.GetAsync("Users");
            if(!response.IsSuccessStatusCode)
            {
                return BadRequest("Error fetch");
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<Users>>(content);
                return Ok(users);

            }
          
        }
       
    }
}
