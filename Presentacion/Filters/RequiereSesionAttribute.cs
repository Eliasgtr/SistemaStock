using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SistemaStock.Web.Filters;

public class RequiereSesionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var usuarioId = context.HttpContext.Session.GetString("UsuarioId");
        if (string.IsNullOrEmpty(usuarioId))
        {
            context.Result = new RedirectToActionResult("Login", "Cuenta", null);
        }
    }
}
