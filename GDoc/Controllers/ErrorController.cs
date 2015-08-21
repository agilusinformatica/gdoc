using GDoc.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GDoc.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
       {
           return new NotFoundActionResult("NotFound");
       }
	}
}