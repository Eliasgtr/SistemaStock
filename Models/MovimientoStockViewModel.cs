using System.ComponentModel.DataAnnotations;

namespace SistemaStock.Models;

public class MovimientoStockViewModel
{
    public int ProductoId { get; set; }

    public string ProductoNombre { get; set; } = null!;

    public string ProductoCodigo { get; set; } = null!;

    public int StockActual { get; set; }

    [Required(ErrorMessage = "Seleccione el tipo de movimiento")]
    [Display(Name = "Tipo de movimiento")]
    public string Tipo { get; set; } = MovimientoStock.Tipos.Entrada;

    [Required(ErrorMessage = "La cantidad es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    [Display(Name = "Cantidad")]
    public int Cantidad { get; set; }

    [Display(Name = "Motivo / Concepto")]
    [StringLength(200)]
    public string? Motivo { get; set; }
}
