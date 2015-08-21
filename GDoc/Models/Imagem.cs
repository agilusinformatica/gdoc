using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDoc.Models
{
    public class Imagem
    {
        public IEnumerable<HttpPostedFileBase> Arquivos { get; set; }
    }
}