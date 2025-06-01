using CatalogCQRS.Application.Features.Products.Queries;
using CatalogCQRS.Domain.Entities;
using Mapster;

namespace CatalogCQRS.Application.Common.Mapping;

public static class MappingConfig
{
    public static void ConfigureMapping()
    {
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        TypeAdapterConfig<Product, GetProductByIdResponse>.NewConfig()
            .Map(dest => dest.Price, src => src.Price);

        // Add more mappings as needed
    }
}
