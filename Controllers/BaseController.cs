using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaStock.Filters;
using SistemaStock.Models;

namespace SistemaStock.Controllers;

[RequiereSesion]
public abstract class BaseController : Controller
{
    protected readonly SistemaStockContext Context;

    protected BaseController(SistemaStockContext context)
    {
        Context = context;
    }

    protected int UsuarioId => int.Parse(HttpContext.Session.GetString("UsuarioId")!);

    protected IQueryable<Categoria> MisCategorias =>
        Context.Categorias.Where(c => c.UsuarioId == UsuarioId);

    protected IQueryable<Producto> MisProductos =>
        Context.Productos.Where(p => p.UsuarioId == UsuarioId);

    protected async Task<Producto?> ObtenerProductoAsync(int id, bool incluirMovimientos = false)
    {
        var query = MisProductos.Include(p => p.Categoria).AsQueryable();

        if (incluirMovimientos)
        {
            query = query.Include(p => p.Movimientos.OrderByDescending(m => m.Fecha));
        }

        return await query.FirstOrDefaultAsync(p => p.ProductoId == id);
    }

    protected async Task<Categoria?> ObtenerCategoriaAsync(int id) =>
        await MisCategorias.FirstOrDefaultAsync(c => c.CategoriaId == id);

    protected async Task<bool> CategoriaEsMiaAsync(int categoriaId) =>
        await MisCategorias.AnyAsync(c => c.CategoriaId == categoriaId);
}
