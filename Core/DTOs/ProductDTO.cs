using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs {
    public class ProductDto {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    
    // TODO!. determine usage!!
    //public class CreateProductCommand {
    //    public string Name { get; set; } = string.Empty;
    //    public decimal Price { get; set; }
    //}

    //public class UpdateProductCommand {
    //    public int Id { get; set; }
    //    public string Name { get; set; } = string.Empty;
    //    public decimal Price { get; set; }
    //}
}
