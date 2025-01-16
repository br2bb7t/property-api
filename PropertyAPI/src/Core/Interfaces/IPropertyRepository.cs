using PropertyAPI.Core.Dtos;

namespace PropertyAPI.Core.Interfaces
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<PropertyDto>> GetPropertiesAsync(string name, string address, decimal? minPrice, decimal? maxPrice);
    }
}
