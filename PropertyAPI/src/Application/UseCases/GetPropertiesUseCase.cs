using PropertyAPI.Core.Interfaces;
using PropertyAPI.Core.Dtos;

namespace PropertyAPI.Application.UseCases
{
    public class GetPropertiesUseCase
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertiesUseCase(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public Task<IEnumerable<PropertyDto>> ExecuteAsync(string name, string address, decimal? minPrice, decimal? maxPrice)
        {
            return _propertyRepository.GetPropertiesAsync(name, address, minPrice, maxPrice);
        }
    }
}
