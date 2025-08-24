using Core.Entities;
using Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Products.Commands {
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int> {
        private readonly IProductRepository _productRepository;
        private readonly IRedisService _redisService;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IRedisService redisService,
            ILogger<CreateProductCommandHandler> logger) {
            _productRepository = productRepository;
            _redisService = redisService;
            _logger = logger;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken) {
            var product = new Product {
                Name = request.Name,
                Price = request.Price,
                CreatedAt = DateTime.UtcNow
            };

            var createdProduct = await _productRepository.AddAsync(product);

            // Cache invalidation
            await _redisService.RemoveAsync("all_products");
            await _redisService.RemoveByPatternAsync("product_*");

            _logger.LogInformation("Product created: {ProductId}", createdProduct.Id);

            return createdProduct.Id;
        }

    }
}
