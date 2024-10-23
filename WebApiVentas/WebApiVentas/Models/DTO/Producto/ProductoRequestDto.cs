namespace WebApiVentas.Models.DTO.Producto
{
    public class ProductoRequestDto
    {
 

        public int Id { get; set; }
        
        public string? Nombre { get; set; }

        public string? Marca { get; set; }

        public decimal? Precio { get; set; }

        public int IdCategoria {  get; set; }



    }
}
