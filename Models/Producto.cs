using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SistemaStock.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int CategoriaId { get; set; }

    public decimal Precio { get; set; }

    public int? Stock { get; set; }

    public int? StockMinimo { get; set; }

    [ValidateNever]
    public virtual Categoria Categoria { get; set; } = null!;
}
