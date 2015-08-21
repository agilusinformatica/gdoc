using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GDoc.MembershipRoleProvider
{
    public class Filtro : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // caso o usuário não possua permissão á página, redireciona ele para página de Acesso Negado
            if (filterContext.Result is HttpUnauthorizedResult && filterContext.HttpContext.Request.IsAuthenticated)
                filterContext.HttpContext.Response.Redirect("/Conta/AcessoNegado");


        }
    }
}