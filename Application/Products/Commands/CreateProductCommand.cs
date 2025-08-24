using MediatR;


namespace Application.Products.Commands {
    public class CreateProductCommand : IRequest<int> {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
