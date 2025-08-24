using Core.DTOs;
using MediatR;

namespace Application.Products.Queries {
    public class GetAllProductsQuery : IRequest<List<ProductDto>> { }
}