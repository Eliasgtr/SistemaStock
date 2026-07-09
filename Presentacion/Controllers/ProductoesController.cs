using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaStock.Domain.Entities;
using SistemaStock.Application.ViewModels;
using SistemaStock.Infrastructure.Persistence;

namespace SistemaStock.Web.Controllers
{
    public class ProductoesController : BaseController
    {
        public ProductoesController(SistemaStockContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            var productos = await MisProductos.Include(p => p.Categoria).ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await ObtenerProductoAsync(id.Value, incluirMovimientos: true);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CategoriaId = new SelectList(await MisCategorias.ToListAsync(), "CategoriaId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Codigo,Nombre,Descripcion,CategoriaId,Precio,Stock,StockMinimo")] Producto producto)
        {
            ModelState.Remove("Categoria");
            ModelState.Remove("Usuario");

            if (!await CategoriaEsMiaAsync(producto.CategoriaId))
            {
                ModelState.AddModelError(nameof(producto.CategoriaId), "La categoría seleccionada no es válida.");
            }

            if (ModelState.IsValid)
            {
                producto.Stock ??= 0;
                producto.StockMinimo ??= 5;
                producto.UsuarioId = UsuarioId;

                Context.Add(producto);
                await Context.SaveChangesAsync();

                if (producto.Stock > 0)
                {
                    Context.MovimientosStock.Add(new MovimientoStock
                    {
                        ProductoId = producto.ProductoId,
                        Tipo = MovimientoStock.Tipos.Entrada,
                        Cantidad = producto.Stock.Value,
                        Motivo = "Stock inicial al crear el producto",
                        Fecha = DateTime.Now,
                        Usuario = HttpContext.Session.GetString("Nombre")
                    });
                    await Context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoriaId = new SelectList(await MisCategorias.ToListAsync(), "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await ObtenerProductoAsync(id.Value);
            if (producto == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(await MisCategorias.ToListAsync(), "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Codigo,Nombre,Descripcion,CategoriaId,Precio,StockMinimo")] Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            ModelState.Remove("Categoria");
            ModelState.Remove("Usuario");

            if (!await CategoriaEsMiaAsync(producto.CategoriaId))
            {
                ModelState.AddModelError(nameof(producto.CategoriaId), "La categoría seleccionada no es válida.");
            }

            if (ModelState.IsValid)
            {
                var productoDb = await MisProductos.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductoId == id);
                if (productoDb == null)
                {
                    return NotFound();
                }

                producto.Stock = productoDb.Stock;
                producto.UsuarioId = productoDb.UsuarioId;

                try
                {
                    Context.Update(producto);
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductoExistsAsync(producto.ProductoId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoriaId"] = new SelectList(await MisCategorias.ToListAsync(), "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        public async Task<IActionResult> Movimiento(int? id, string? tipo)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await ObtenerProductoAsync(id.Value);
            if (producto == null)
            {
                return NotFound();
            }

            var model = new MovimientoStockViewModel
            {
                ProductoId = producto.ProductoId,
                ProductoNombre = producto.Nombre,
                ProductoCodigo = producto.Codigo,
                StockActual = producto.Stock ?? 0,
                Tipo = tipo == MovimientoStock.Tipos.Salida
                    ? MovimientoStock.Tipos.Salida
                    : MovimientoStock.Tipos.Entrada
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Movimiento(MovimientoStockViewModel model)
        {
            var producto = await ObtenerProductoAsync(model.ProductoId);
            if (producto == null)
            {
                return NotFound();
            }

            model.ProductoNombre = producto.Nombre;
            model.ProductoCodigo = producto.Codigo;
            model.StockActual = producto.Stock ?? 0;

            if (model.Tipo == MovimientoStock.Tipos.Salida && model.Cantidad > model.StockActual)
            {
                ModelState.AddModelError(nameof(model.Cantidad),
                    $"No hay suficiente stock. Disponible: {model.StockActual} unidades.");
            }

            if (ModelState.IsValid)
            {
                var stockActual = producto.Stock ?? 0;

                if (model.Tipo == MovimientoStock.Tipos.Entrada)
                {
                    producto.Stock = stockActual + model.Cantidad;
                }
                else
                {
                    producto.Stock = stockActual - model.Cantidad;
                }

                Context.MovimientosStock.Add(new MovimientoStock
                {
                    ProductoId = producto.ProductoId,
                    Tipo = model.Tipo,
                    Cantidad = model.Cantidad,
                    Motivo = model.Motivo,
                    Fecha = DateTime.Now,
                    Usuario = HttpContext.Session.GetString("Nombre")
                });

                Context.Update(producto);
                await Context.SaveChangesAsync();

                TempData["Mensaje"] = model.Tipo == MovimientoStock.Tipos.Entrada
                    ? $"Entrada de {model.Cantidad} unidades registrada correctamente."
                    : $"Salida de {model.Cantidad} unidades registrada correctamente.";

                return RedirectToAction(nameof(Details), new { id = producto.ProductoId });
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await ObtenerProductoAsync(id.Value);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await ObtenerProductoAsync(id);
            if (producto != null)
            {
                Context.Productos.Remove(producto);
                await Context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductoExistsAsync(int id) =>
            await MisProductos.AnyAsync(e => e.ProductoId == id);
    }
}
