using System;
using System.Collections.Generic;

namespace SistemaStock.Domain.Entities;

public partial class MovimientoStock
{
    public int MovimientoId { get; set; }

    public int ProductoId { get; set; }

    public string Tipo { get; set; } = null!;

    public int Cantidad { get; set; }

    public string? Motivo { get; set; }

    public DateTime Fecha { get; set; }

    public string? Usuario { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public static class Tipos
    {
        public const string Entrada = "Entrada";
        public const string Salida = "Salida";
    }
}
