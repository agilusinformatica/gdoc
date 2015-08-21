using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDoc.ViewModels
{
    public class ItemDocumento
    {
        public int Id { get; set; }
        public bool Selecionado { get; set; }
        public string Nome { get; set; }
        public bool Obrigatorio { get; set; }
                
    }
    
    public class BancoViewModel
    {
        public BancoViewModel()
        {
            this.Documentos = new List<ItemDocumento>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public List<ItemDocumento> Documentos { get; set; }
    }
}