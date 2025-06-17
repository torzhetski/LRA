using LRA.Shared.Abstructions.Test;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Interfaces;
using System.Text.Json;
using System.Text;

namespace Gateways.ClientGateway.Controllers
{
    [Route("api/gateway")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IHttpClientService _http;

        public GatewayController(IHttpClientService http)
        {
            _http = http;
        }

        [HttpGet("auth")]
        public async Task<TestMessageModel> GetAuthResponseAsync([FromQuery] int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7004/api/test-auth?id={id}");
            return await _http.SendAsync<TestMessageModel>(request);
        }

        [HttpGet("account")]
        public async Task<TestMessageModel> GetAccountResponseAsync([FromQuery] int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7003/api/test-account?id={id}");
            return await _http.SendAsync<TestMessageModel>(request);
        }

        [HttpPost("account")]
        public async Task<TestMessageModel> CreateAccountAsync([FromBody] TestModel model)
        {
            var json = JsonSerializer.Serialize(model);
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7003/api/test-account")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            return await _http.SendAsync<TestMessageModel>(request);
        }
    }
}
