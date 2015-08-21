using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GDoc.ViewModels
{
    public class ItemAcesso 
    {
        public int IdFuncao { get; set; }
        public string Nome { get; set; }
        public bool Selecionado { get; set; }
        public int? IdFuncaoSuperior { get; set; }
    }

    public class PerfilViewModel
    {
        public PerfilViewModel()
        {
            this.Acessos = new List<ItemAcesso>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        [DisplayName("Limita Propostas do Usuário Logado")]        
        public bool LimitaPropostaUsuarioLogado { get; set; }

        public List<ItemAcesso> Acessos { get; set; }
    }

}