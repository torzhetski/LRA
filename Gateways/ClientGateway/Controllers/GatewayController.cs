using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions.Interfaces;

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
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7183/api/test-auth?id={id}");
            return await _http.SendAsync<TestMessageModel>(request);
        }

        [HttpGet("account")]
        public async Task<TestMessageModel> GetAccountResponseAsync([FromQuery] int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7052/api/test-account?id={id}");
            return await _http.SendAsync<TestMessageModel>(request);
        }
    }
}
