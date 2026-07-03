using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaStock.Models;

namespace SistemaStock.Controllers
{
    public class CategoriasController : BaseController
    {
        public CategoriasController(SistemaStockContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View(await MisCategorias.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await ObtenerCategoriaAsync(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                categoria.UsuarioId = UsuarioId;
                Context.Add(categoria);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await ObtenerCategoriaAsync(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return NotFound();
            }

            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                var categoriaDb = await ObtenerCategoriaAsync(id);
                if (categoriaDb == null)
                {
                    return NotFound();
                }

                categoria.UsuarioId = categoriaDb.UsuarioId;

                try
                {
                    Context.Update(categoria);
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoriaExistsAsync(categoria.CategoriaId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await ObtenerCategoriaAsync(id.Value);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await ObtenerCategoriaAsync(id);
            if (categoria != null)
            {
                Context.Categorias.Remove(categoria);
                await Context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoriaExistsAsync(int id) =>
            await MisCategorias.AnyAsync(e => e.CategoriaId == id);
    }
}
