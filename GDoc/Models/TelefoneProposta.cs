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
    
    public partial class TelefoneProposta
    {
        public int Id { get; set; }
        public int IdProposta { get; set; }
        public Nullable<int> IdTipoTelefone { get; set; }
        public string Telefone { get; set; }
    
        public virtual Proposta Proposta { get; set; }
        public virtual TipoTelefone TipoTelefone { get; set; }
    }
}
