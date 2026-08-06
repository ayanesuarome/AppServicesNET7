using AutoMapper;
using AutoMapper.Internal;
using MappingObjects.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MappingObjects.Mappers;

public static class CartToSummaryMapper
{
    public static MapperConfiguration GetMapperConfiguration()
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;

            cfg.CreateMap<Cart, Summary>()
                // FullName
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    $"{src.Customer.FirstName} {src.Customer.LastName}"))
                
                // Total
                .ForMember(dest => dest.Total, opt => opt.MapFrom(
                    src => src.Items.Sum(item => item.UnitPrice * item.Quantity)));
        }, NullLoggerFactory.Instance);

        return config;
    }
}
