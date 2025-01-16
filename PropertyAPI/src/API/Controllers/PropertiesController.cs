using Microsoft.AspNetCore.Mvc;
using PropertyAPI.Application.UseCases;
using PropertyAPI.Core.Dtos;

namespace PropertyAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly GetPropertiesUseCase _getPropertiesUseCase;

        public PropertiesController(GetPropertiesUseCase getPropertiesUseCase)
        {
            _getPropertiesUseCase = getPropertiesUseCase;
        }

        [HttpGet] // Este es el endpoint GET
        public async Task<IActionResult> GetProperties([FromQuery] string name, [FromQuery] string address, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var properties = await _getPropertiesUseCase.ExecuteAsync(name, address, minPrice, maxPrice);
            return Ok(properties);
        }
    }
}
