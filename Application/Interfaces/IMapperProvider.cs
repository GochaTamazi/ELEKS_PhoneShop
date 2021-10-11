using AutoMapper;

namespace Application.Interfaces
{
    public interface IMapperProvider
    {
        IMapper GetMapper();
    }
}