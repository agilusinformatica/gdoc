//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GDoc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Banco
    {
        public Banco()
        {
            this.BancoDocumentos = new HashSet<BancoDocumento>();
            this.Dividas = new HashSet<Divida>();
            this.PropostasBancos = new HashSet<Proposta>();
            this.Propostas = new HashSet<Proposta>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
    
        public virtual ICollection<BancoDocumento> BancoDocumentos { get; set; }
        public virtual ICollection<Divida> Dividas { get; set; }
        public virtual ICollection<Proposta> PropostasBancos { get; set; }
        public virtual ICollection<Proposta> Propostas { get; set; }
    }
}
