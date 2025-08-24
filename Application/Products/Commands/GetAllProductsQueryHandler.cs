using Application.Products.Queries;
using Core.DTOs;
using Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands {

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>> {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(
            IProductRepository productRepository,
            IRedisService redisService,
            ILogger<GetAllProductsQueryHandler> logger) {
            _productRepository = productRepository;
            _redisService = redisService;
            _logger = logger;
        }
        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken) {
                        const string cacheKey = "all_products";
            
            // Check cache first
            var cachedProducts = await _redisService.GetAsync<List<ProductDto>>(cacheKey);
            if (cachedProducts != null)
            {
                _logger.LogInformation("Returning cached products");
                return cachedProducts;
            }

            // Get from database
            var products = await _productRepository.GetAllAsync();
            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();

            // Cache for 5 minutes
            await _redisService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
            _logger.LogInformation("Products cached with key: {CacheKey}", cacheKey);
            
            return result;
        }

    }
}
