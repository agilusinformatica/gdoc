using GDoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GDoc.ViewModels
{
    public class AnexosViewModel
    {
        public string PastaRaiz { get; set; }
        public int? TaxaCompressao { get; set; }
        public int IdProposta { get; set; }

        public List<DocumentoProposta> Imagens { get; set; }
        public List<BancoDocumento> Docs { get; set; }
    }
}