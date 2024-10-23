using System;
using System.Collections.Generic;

namespace WebApiVentas.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int? IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public string? Marca { get; set; }

    public decimal? Precio { get; set; }

    public DateOnly? Fecha { get; set; }

    public virtual Categoria? IdCategoriaNavigation { get; set; }
}
